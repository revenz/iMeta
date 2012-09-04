using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
//using MediaInfoLib;
using System.Text.RegularExpressions;

namespace iMetaLibrary.Metadata
{
    [Serializable]
    public class VideoFileMeta
    {
        public VideoTrack Video { get; set; }
        public AudioTrack[] Audio { get; set; }
        public SubtitleTrack[] Subtitles { get; set; }
        public VideoFileMeta()
        {
            this.Video = new VideoTrack();
            this.Audio = new AudioTrack[] { };
            this.Subtitles = new SubtitleTrack[] { };
        }

        public XElement CreateXElement()
        {
            XElement fileinfo = new XElement("fileinfo");
            XElement streamdetails = new XElement("streamdetails");
            if (Video != null)
            {
                XElement velement = Video.CreateXElement();
                if (velement != null)
                    streamdetails.Add(velement);
            }
            if (this.Audio != null)
            {
                foreach (AudioTrack at in this.Audio)
                {
                    XElement atelement = at.CreateXElement();
                    if (atelement != null)
                        streamdetails.Add(atelement);
                }
            }
            if (this.Subtitles != null)
            {
                foreach (SubtitleTrack st in this.Subtitles)
                {
                    XElement stelement = st.CreateXElement();
                    if (stelement != null)
                        streamdetails.Add(stelement);
                }
            }
            fileinfo.Add(streamdetails);
            return fileinfo;
        }

        internal static VideoFileMeta Load(string Filename)
        {
			return null;
			/*
            MediaInfo MI = new MediaInfo();
            if (MI.Option("Info_Version", "0.7.0.0;MediaInfoDLL_Example_CS;0.7.0.0").Length < 1)
                return null;
            VideoFileMeta fileMeta = new VideoFileMeta();
            MI.Open(Filename);

            MI.Option("Complete");
            MI.Option("Complete", "1");
            MI.Option("Inform", "General;File size is %FileSize% bytes");

            if (MI.Count_Get(StreamKind.Video) > 0)
            {
                //string bitrate = MI.Get(StreamKind.Video, 0, "BitRate");
                float aspect = 0;
                if (float.TryParse(MI.Get(StreamKind.Video, 0, "AspectRatio"), out aspect))
                    fileMeta.Video.Aspect = aspect;
                fileMeta.Video.Codec = MI.Get(StreamKind.Video, 0, "Codec");
                int width = 0;
                if (int.TryParse(MI.Get(StreamKind.Video, 0, "Width"), out width))
                    fileMeta.Video.Width = width;
                int height = 0;
                if (int.TryParse(MI.Get(StreamKind.Video, 0, "Height"), out height))
                    fileMeta.Video.Height = height;
                int duration = 0;
                if (int.TryParse(MI.Get(StreamKind.Video, 0, "Duration"), out duration)) // gives duration in milliseconds
                    fileMeta.Video.DurationInSeconds = duration / 1000;
            }

            int audioTracks = MI.Count_Get(StreamKind.Audio);
            fileMeta.Audio = new AudioTrack[audioTracks];
            for (int i = 0; i < audioTracks; i++)
            {
                fileMeta.Audio[i] = new AudioTrack();
                fileMeta.Audio[i].Codec = MI.Get(StreamKind.Audio, i, "Codec");
                fileMeta.Audio[i].Language = MI.Get(StreamKind.Audio, i, "Language");
                float channels = 0;
                if (float.TryParse(MI.Get(StreamKind.Audio, i, "Channels"), out channels))
                    fileMeta.Audio[i].Channels = channels;
            }

            int subtitles = MI.Count_Get(StreamKind.Text);
            fileMeta.Subtitles = new SubtitleTrack[subtitles];
            for (int i = 0; i < subtitles; i++)
            {
                fileMeta.Subtitles[i] = new SubtitleTrack();
                fileMeta.Subtitles[i].Language = MI.Get(StreamKind.Text, i, "Language");
            }

            MI.Close();
            return fileMeta;
            */
        }

        public static VideoFileMeta Load(XElement Element)
        {
            try
            {
                VideoFileMeta meta = new VideoFileMeta();
                // load video track
                if (Element.Element("streamdetails").Element("video") != null)
                {
                    VideoTrack video = new VideoTrack();
                    if (NfoLoader.Load<VideoTrack>(video, Element.Element("streamdetails").Element("video")))
                        meta.Video = video;
                }
                // load audio tracks
                foreach(XElement eleAudio in Element.Element("streamdetails").Elements("audio"))
                {
                    AudioTrack audio = new AudioTrack();
                    if (NfoLoader.Load<AudioTrack>(audio, eleAudio))
                        meta.Audio = meta.Audio.Union(new AudioTrack[] { audio }).ToArray();
                }
                // load subtitle tracks
                foreach (XElement eleSubtitle in Element.Element("streamdetails").Elements("subtitle"))
                {
                    SubtitleTrack subtitle = new SubtitleTrack();
                    if (NfoLoader.Load<SubtitleTrack>(subtitle, eleSubtitle))
                        meta.Subtitles = meta.Subtitles.Union(new SubtitleTrack[] { subtitle }).ToArray();
                }
                return meta;
            }
            catch (Exception) { return null; }
        }
    }

    [Serializable]
    public class VideoTrack
    {
        public string Codec { get; set; }
        public float Aspect { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int DurationInSeconds { get; set; }

        public XElement CreateXElement()
        {
            XElement element = new XElement("video");
            if (!String.IsNullOrEmpty(Codec))
            {
                string codecString = this.Codec;
                if (this.Codec.ToLower().Contains("h264"))
                    codecString = "h264";
                element.Add(new XElement("codec", codecString));
            }
            if (this.Aspect > 0)
                element.Add(new XElement("aspect", Regex.Match(this.Aspect.ToString(), @"^[\d]+(\.[0-9]*[1-9]{1})?")));
            if (this.Width > 0)
                element.Add(new XElement("width", this.Width.ToString()));
            if (this.Height > 0)
                element.Add(new XElement("height", this.Height.ToString()));
            if (this.DurationInSeconds > 0)
                element.Add(new XElement("durationinseconds", this.DurationInSeconds.ToString()));
            return element;
        }
    }

    [Serializable]
    public class AudioTrack
    {
        public string Codec { get; set; }
        public string Language { get; set; }
        public float Channels { get; set; }

        public XElement CreateXElement()
        {
            XElement element = new XElement("audio");
            if (!String.IsNullOrEmpty(Codec))
                element.Add(new XElement("codec", this.Codec));
            if (!String.IsNullOrEmpty(Language))
                element.Add(new XElement("language", this.Language));
            if (Channels > 0)
                element.Add(new XElement("channels", Regex.Match(this.Channels.ToString(), @"^[\d]+(\.[0-9]*[1-9]{1})?")));
            return element;
        }
    }

    [Serializable]
    public class SubtitleTrack
    {
        public string Language { get; set; }

        public XElement CreateXElement()
        {
            if (String.IsNullOrEmpty(Language))
                return null;
            XElement element = new XElement("subtitle");
            if (!String.IsNullOrEmpty(this.Language))
                element.Add(new XElement("language", this.Language));
            return element;
        }
    }
}
