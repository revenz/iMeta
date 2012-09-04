﻿/*
 *   TvdbLib: A library to retrieve information and media from http://thetvdb.com
 * 
 *   Copyright (C) 2008  Benjamin Gmeiner
 * 
 *   This program is free software: you can redistribute it and/or modify
 *   it under the terms of the GNU General Public License as published by
 *   the Free Software Foundation, either version 3 of the License, or
 *   (at your option) any later version.
 *
 *   This program is distributed in the hope that it will be useful,
 *   but WITHOUT ANY WARRANTY; without even the implied warranty of
 *   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *   GNU General Public License for more details.
 *
 *   You should have received a copy of the GNU General Public License
 *   along with this program.  If not, see <http://www.gnu.org/licenses/>.
 * 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TvdbLib.Data.Banner
{
  /// <summary>
  /// Represents the episode banner, which is currently only one image
  /// per episode (no language differentiation either) limited to a maximum 
  /// size of 400 x 300 
  /// 
  /// further information on http://thetvdb.com/wiki/index.php/Episode_Images
  /// </summary>
  [Serializable]
  public class TvdbEpisodeBanner: TvdbBannerWithThumb
  {
    /// <summary>
    /// TvdbEpisodeBanner constructor
    /// </summary>
    public TvdbEpisodeBanner()
      : base()
    {
      this.Language = new TvdbLanguage(-99, "Universal, valid for all languages", "all");
    }

    /// <summary>
    /// TvdbEpisodeBanner constructor
    /// </summary>
    /// <param name="_bannerPath">Path of banner</param>
    /// <param name="_id">Id of episode banner</param>
    public TvdbEpisodeBanner(int _id, String _bannerPath):this()
    {
      Id = _id;
      BannerPath = _bannerPath;
    }
  }
}
