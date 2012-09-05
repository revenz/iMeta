using System;

namespace iMetaGui
{
	public class Images
	{
		private static Gdk.Pixbuf _Empty;
		public static Gdk.Pixbuf Empty
		{
			get
			{
				if(_Empty == null)					
				{
					_Empty = new Gdk.Pixbuf(Gdk.Colorspace.Rgb,  true, 8, 1, 1);
				}
				return _Empty;
			}
		}
		
		private static Gdk.Pixbuf _TopBarBackground;
		public static Gdk.Pixbuf TopBarBackground
		{
			get
			{
				if(_TopBarBackground == null)					
				{
					_TopBarBackground = new Gdk.Pixbuf(null, "iMetaGui.Images.topbarbackground.png");
				}
				return _TopBarBackground;
			}
		}
		private static Gdk.Pixbuf _DialogBackground;
		public static Gdk.Pixbuf DialogBackground
		{
			get
			{
				if(_DialogBackground == null)					
				{
					_DialogBackground = new Gdk.Pixbuf(null, "iMetaGui.Images.dialogbackground.png");
				}
				return _DialogBackground;
			}
		}
		private static Gdk.Pixbuf _LeftPaneBackground;
		public static Gdk.Pixbuf LeftPaneBackground
		{
			get
			{
				if(_LeftPaneBackground == null)					
				{
					_LeftPaneBackground = new Gdk.Pixbuf(null, "iMetaGui.Images.leftpanebackground.png");
				}
				return _LeftPaneBackground;
			}
		}
		#region ibutton		
		private static Gdk.Pixbuf _iButtonLeft;
		public static Gdk.Pixbuf iButtonLeft
		{
			get
			{
				if(_iButtonLeft == null)					
				{
					_iButtonLeft = new Gdk.Pixbuf(null, "iMetaGui.Images.ibuttonleft.png");
				}
				return _iButtonLeft;
			}
		}	
		private static Gdk.Pixbuf _iButtonLeftPushed;
		public static Gdk.Pixbuf iButtonLeftPushed
		{
			get
			{
				if(_iButtonLeftPushed == null)					
				{
					_iButtonLeftPushed = new Gdk.Pixbuf(null, "iMetaGui.Images.ibuttonleft_pushed.png");
				}
				return _iButtonLeftPushed;
			}
		}	
		private static Gdk.Pixbuf _iButtonRight;
		public static Gdk.Pixbuf iButtonRight
		{
			get
			{
				if(_iButtonRight == null)					
				{
					_iButtonRight = new Gdk.Pixbuf(null, "iMetaGui.Images.ibuttonright.png");
				}
				return _iButtonRight;
			}
		}
		private static Gdk.Pixbuf _iButtonRightPushed;
		public static Gdk.Pixbuf iButtonRightPushed
		{
			get
			{
				if(_iButtonRightPushed == null)					
				{
					_iButtonRightPushed = new Gdk.Pixbuf(null, "iMetaGui.Images.ibuttonright_pushed.png");
				}
				return _iButtonRightPushed;
			}
		}	
		#endregion
		
		#region status widget
		private static Gdk.Pixbuf _StatusWidget_Left;
		public static Gdk.Pixbuf StatusWidget_Left
		{
			get
			{
				if(_StatusWidget_Left == null)			
					_StatusWidget_Left = new Gdk.Pixbuf(null, "iMetaGui.Images.statuswidget_left.png");
				return _StatusWidget_Left;
			}
		}	
		private static Gdk.Pixbuf _StatusWidget_Right;
		public static Gdk.Pixbuf StatusWidget_Right
		{
			get
			{
				if(_StatusWidget_Right == null)			
					_StatusWidget_Right = new Gdk.Pixbuf(null, "iMetaGui.Images.statuswidget_right.png");
				return _StatusWidget_Right;
			}
		}
		private static Gdk.Pixbuf _StatusWidget_Background;
		public static Gdk.Pixbuf StatusWidget_Background
		{
			get
			{
				if(_StatusWidget_Background == null)			
					_StatusWidget_Background = new Gdk.Pixbuf(null, "iMetaGui.Images.statuswidget_background.png");
				return _StatusWidget_Background;
			}
		}
		#endregion
		
		#region windows buttons
		private static Gdk.Pixbuf _CloseBackground;
		public static Gdk.Pixbuf CloseBackground
		{
			get
			{
				if(_CloseBackground == null)					
				{
					_CloseBackground = new Gdk.Pixbuf(null, "iMetaGui.Images.closebackground.png");
				}
				return _CloseBackground;
			}
		}
		
		private static Gdk.Pixbuf _Windows_CloseButton;
		public static Gdk.Pixbuf Windows_CloseButton
		{
			get
			{
				if(_Windows_CloseButton == null)					
				{
					_Windows_CloseButton = new Gdk.Pixbuf(null, "iMetaGui.Images.windows_closebutton.png");
				}
				return _Windows_CloseButton;
			}
		}
		
		private static Gdk.Pixbuf _Windows_CloseButton_Inactive;
		public static Gdk.Pixbuf Windows_CloseButton_Inactive
		{
			get
			{
				if(_Windows_CloseButton_Inactive == null)					
				{
					_Windows_CloseButton_Inactive = new Gdk.Pixbuf(null, "iMetaGui.Images.windows_closebutton_inactive.png");
				}
				return _Windows_CloseButton_Inactive;
			}
		}
		private static Gdk.Pixbuf _Windows_CloseButton_Hover;
		public static Gdk.Pixbuf Windows_CloseButton_Hover
		{
			get
			{
				if(_Windows_CloseButton_Hover == null)					
				{
					_Windows_CloseButton_Hover = new Gdk.Pixbuf(null, "iMetaGui.Images.windows_closebutton_hover.png");
				}
				return _Windows_CloseButton_Hover;
			}
		}
		private static Gdk.Pixbuf _Windows_MaximizeButton;
		public static Gdk.Pixbuf Windows_MaximizeButton
		{
			get
			{
				if(_Windows_MaximizeButton == null)					
				{
					_Windows_MaximizeButton = new Gdk.Pixbuf(null, "iMetaGui.Images.windows_maximizebutton.png");
				}
				return _Windows_MaximizeButton;
			}
		}
		private static Gdk.Pixbuf _Windows_MaximizeButton_Hover;
		public static Gdk.Pixbuf Windows_MaximizeButton_Hover
		{
			get
			{
				if(_Windows_MaximizeButton_Hover == null)					
				{
					_Windows_MaximizeButton_Hover = new Gdk.Pixbuf(null, "iMetaGui.Images.windows_maximizebutton_hover.png");
				}
				return _Windows_MaximizeButton_Hover;
			}
		}
		private static Gdk.Pixbuf _Windows_MinimizeButton;
		public static Gdk.Pixbuf Windows_MinimizeButton
		{
			get
			{
				if(_Windows_MinimizeButton == null)					
				{
					_Windows_MinimizeButton = new Gdk.Pixbuf(null, "iMetaGui.Images.windows_minimizebutton.png");
				}
				return _Windows_MinimizeButton;
			}
		}
		private static Gdk.Pixbuf _Windows_MinimizeButton_Hover;
		public static Gdk.Pixbuf Windows_MinimizeButton_Hover
		{
			get
			{
				if(_Windows_MinimizeButton_Hover == null)					
				{
					_Windows_MinimizeButton_Hover = new Gdk.Pixbuf(null, "iMetaGui.Images.windows_minimizebutton_hover.png");
				}
				return _Windows_MinimizeButton_Hover;
			}
		}
		
		#endregion
		
		
		#region mac buttons		
		private static Gdk.Pixbuf _Mac_CloseButton;
		public static Gdk.Pixbuf Mac_CloseButton
		{
			get
			{
				if(_Mac_CloseButton == null)					
				{
					_Mac_CloseButton = new Gdk.Pixbuf(null, "iMetaGui.Images.mac_closebutton.png");
				}
				return _Mac_CloseButton;
			}
		}
		private static Gdk.Pixbuf _Mac_CloseButton_Hover;
		public static Gdk.Pixbuf Mac_CloseButton_Hover
		{
			get
			{
				if(_Mac_CloseButton_Hover == null)					
				{
					_Mac_CloseButton_Hover = new Gdk.Pixbuf(null, "iMetaGui.Images.mac_closebutton_hover.png");
				}
				return _Mac_CloseButton_Hover;
			}
		}
		private static Gdk.Pixbuf _Mac_MaximizeButton;
		public static Gdk.Pixbuf Mac_MaximizeButton
		{
			get
			{
				if(_Mac_MaximizeButton == null)					
				{
					_Mac_MaximizeButton = new Gdk.Pixbuf(null, "iMetaGui.Images.mac_maximizebutton.png");
				}
				return _Mac_MaximizeButton;
			}
		}
		private static Gdk.Pixbuf _Mac_MaximizeButton_Hover;
		public static Gdk.Pixbuf Mac_MaximizeButton_Hover
		{
			get
			{
				if(_Mac_MaximizeButton_Hover == null)					
				{
					_Mac_MaximizeButton_Hover = new Gdk.Pixbuf(null, "iMetaGui.Images.mac_maximizebutton_hover.png");
				}
				return _Mac_MaximizeButton_Hover;
			}
		}
		private static Gdk.Pixbuf _Mac_MinimizeButton;
		public static Gdk.Pixbuf Mac_MinimizeButton
		{
			get
			{
				if(_Mac_MinimizeButton == null)					
				{
					_Mac_MinimizeButton = new Gdk.Pixbuf(null, "iMetaGui.Images.mac_minimizebutton.png");
				}
				return _Mac_MinimizeButton;
			}
		}
		private static Gdk.Pixbuf _Mac_MinimizeButton_Hover;
		public static Gdk.Pixbuf Mac_MinimizeButton_Hover
		{
			get
			{
				if(_Mac_MinimizeButton_Hover == null)					
				{
					_Mac_MinimizeButton_Hover = new Gdk.Pixbuf(null, "iMetaGui.Images.mac_minimizebutton_hover.png");
				}
				return _Mac_MinimizeButton_Hover;
			}
		}
		
		#endregion
		
		#region no posters
		private static System.Drawing.Image _NoPosterImage = null;
		public static System.Drawing.Image NoPosterImage
		{
			get
			{
				if(_NoPosterImage == null)
				{
					byte[] data = NoPosterPixbuf.SaveToBuffer("png");	
					using(System.IO.MemoryStream stream = new System.IO.MemoryStream(data))
					{
						stream.Position = 0;
						_NoPosterImage = System.Drawing.Image.FromStream(stream);
					}
				}
				return _NoPosterImage;
			}
		}
		private static System.Drawing.Image _NoPosterSmallImage = null;
		public static System.Drawing.Image NoPosterSmallImage
		{
			get
			{
				if(_NoPosterSmallImage == null)
				{
					byte[] data = NoPosterSmallPixbuf.SaveToBuffer("png");	
					using(System.IO.MemoryStream stream = new System.IO.MemoryStream(data))
					{
						stream.Position = 0;
						_NoPosterSmallImage = System.Drawing.Image.FromStream(stream);
					}
				}
				return _NoPosterSmallImage;
			}
		}
		
		private static Gdk.Pixbuf _NoPosterPixbuf;
		public static Gdk.Pixbuf NoPosterPixbuf			
		{
			get
			{
				if(_NoPosterPixbuf == null)					
				{
					_NoPosterPixbuf = new Gdk.Pixbuf(null, "iMetaGui.Images.noposter.png");
				}
				return _NoPosterPixbuf;
			}
		}
		private static Gdk.Pixbuf _NoPosterSmallPixbuf;
		public static Gdk.Pixbuf NoPosterSmallPixbuf			
		{
			get
			{
				if(_NoPosterSmallPixbuf == null)					
				{
					_NoPosterSmallPixbuf = new Gdk.Pixbuf(null, "iMetaGui.Images.noposter_small.png");
				}
				return _NoPosterSmallPixbuf;
			}
		}
		#endregion
		
		#region stars
		private static Gdk.Pixbuf _Stars00;
		public static Gdk.Pixbuf Stars00
		{
			get
			{
				if(_Stars00 == null)					
				{
					_Stars00 = new Gdk.Pixbuf(null, "iMetaGui.Images.stars_00.png");
				}
				return _Stars00;
			}
		}
		private static Gdk.Pixbuf _Stars05;
		public static Gdk.Pixbuf Stars05
		{
			get
			{
				if(_Stars05 == null)					
				{
					_Stars05 = new Gdk.Pixbuf(null, "iMetaGui.Images.stars_05.png");
				}
				return _Stars05;
			}
		}
		private static Gdk.Pixbuf _Stars10;
		public static Gdk.Pixbuf Stars10
		{
			get
			{
				if(_Stars10 == null)					
				{
					_Stars10 = new Gdk.Pixbuf(null, "iMetaGui.Images.stars_10.png");
				}
				return _Stars10;
			}
		}
		private static Gdk.Pixbuf _Stars15;
		public static Gdk.Pixbuf Stars15
		{
			get
			{
				if(_Stars15 == null)					
				{
					_Stars15 = new Gdk.Pixbuf(null, "iMetaGui.Images.stars_15.png");
				}
				return _Stars15;
			}
		}
		private static Gdk.Pixbuf _Stars20;
		public static Gdk.Pixbuf Stars20
		{
			get
			{
				if(_Stars20 == null)					
				{
					_Stars20 = new Gdk.Pixbuf(null, "iMetaGui.Images.stars_20.png");
				}
				return _Stars20;
			}
		}
		private static Gdk.Pixbuf _Stars25;
		public static Gdk.Pixbuf Stars25
		{
			get
			{
				if(_Stars25 == null)					
				{
					_Stars25 = new Gdk.Pixbuf(null, "iMetaGui.Images.stars_25.png");
				}
				return _Stars25;
			}
		}
		private static Gdk.Pixbuf _Stars30;
		public static Gdk.Pixbuf Stars30
		{
			get
			{
				if(_Stars30 == null)					
				{
					_Stars30 = new Gdk.Pixbuf(null, "iMetaGui.Images.stars_30.png");
				}
				return _Stars30;
			}
		}
		private static Gdk.Pixbuf _Stars35;
		public static Gdk.Pixbuf Stars35
		{
			get
			{
				if(_Stars35 == null)					
				{
					_Stars35 = new Gdk.Pixbuf(null, "iMetaGui.Images.stars_35.png");
				}
				return _Stars35;
			}
		}
		private static Gdk.Pixbuf _Stars40;
		public static Gdk.Pixbuf Stars40
		{
			get
			{
				if(_Stars40 == null)					
				{
					_Stars40 = new Gdk.Pixbuf(null, "iMetaGui.Images.stars_40.png");
				}
				return _Stars40;
			}
		}
		private static Gdk.Pixbuf _Stars45;
		public static Gdk.Pixbuf Stars45
		{
			get
			{
				if(_Stars45 == null)					
				{
					_Stars45 = new Gdk.Pixbuf(null, "iMetaGui.Images.stars_45.png");
				}
				return _Stars45;
			}
		}
		private static Gdk.Pixbuf _Stars50;
		public static Gdk.Pixbuf Stars50
		{
			get
			{
				if(_Stars50 == null)					
				{
					_Stars50 = new Gdk.Pixbuf(null, "iMetaGui.Images.stars_50.png");
				}
				return _Stars50;
			}
		}
		private static Gdk.Pixbuf _Star = null;
		public static Gdk.Pixbuf Star
		{
			get
			{
				if(_Star == null)					
				{
					_Star = new Gdk.Pixbuf(null, "iMetaGui.Images.star.png");
				}
				return _Star;
			}
		}
		private static Gdk.Pixbuf _StarEmpty = null;
		public static Gdk.Pixbuf StarEmpty
		{
			get
			{
				if(_StarEmpty == null)					
				{
					_StarEmpty = new Gdk.Pixbuf(null, "iMetaGui.Images.starempty.png");
				}
				return _StarEmpty;
			}
		}
		private static Gdk.Pixbuf _Star32;
		public static Gdk.Pixbuf Star32
		{
			get
			{
				if(_Star32 == null)					
				{
					_Star32 = new Gdk.Pixbuf(null, "iMetaGui.Images.star32.png");
				}
				return _Star32;
			}
		}
		private static Gdk.Pixbuf _StarEmpty32;
		public static Gdk.Pixbuf StarEmpty32
		{
			get
			{
				if(_StarEmpty32 == null)					
				{
					_StarEmpty32 = new Gdk.Pixbuf(null, "iMetaGui.Images.starempty32.png");
				}
				return _StarEmpty32;
			}
		}
		private static Gdk.Pixbuf _StarHalf32;
		public static Gdk.Pixbuf StarHalf32
		{
			get
			{
				if(_StarHalf32 == null)					
				{
					_StarHalf32 = new Gdk.Pixbuf(null, "iMetaGui.Images.starhalf32.png");
				}
				return _StarHalf32;
			}
		}
		#endregion
		
		#region status icons		
		private static Gdk.Pixbuf _StatusGreen = null;
		public static Gdk.Pixbuf StatusGreen
		{
			get
			{
				if(_StatusGreen == null)					
				{
					_StatusGreen = new Gdk.Pixbuf(null, "iMetaGui.Images.green.png");
				}
				return _StatusGreen;
			}
		}	
		private static Gdk.Pixbuf _StatusOrange = null;
		public static Gdk.Pixbuf StatusOrange
		{
			get
			{
				if(_StatusOrange == null)					
				{
					_StatusOrange = new Gdk.Pixbuf(null, "iMetaGui.Images.orange.png");
				}
				return _StatusOrange;
			}
		}	
		private static Gdk.Pixbuf _StatusRed = null;
		public static Gdk.Pixbuf StatusRed
		{
			get
			{
				if(_StatusRed == null)					
				{
					_StatusRed = new Gdk.Pixbuf(null, "iMetaGui.Images.red.png");
				}
				return _StatusRed;
			}
		}	
		private static Gdk.Pixbuf _StatusLoading = null;
		public static Gdk.Pixbuf StatusLoading
		{
			get
			{
				if(_StatusLoading == null)					
				{
					_StatusLoading = new Gdk.Pixbuf(null, "iMetaGui.Images.loading.png");
				}
				return _StatusLoading;
			}
		}
		#endregion

		#region Preferences tab icons
		private static Gdk.Pixbuf _TabAdvanced;
		public static Gdk.Pixbuf TabAdvanced
		{
			get
			{
				if(_TabAdvanced == null)			
					_TabAdvanced = new Gdk.Pixbuf(null, "iMetaGui.Images.advanced.png");
				return _TabAdvanced;
			}
		}
		private static Gdk.Pixbuf _TabAdvancedInactive;
		public static Gdk.Pixbuf TabAdvancedInactive
		{
			get
			{
				if(_TabAdvancedInactive == null)			
					_TabAdvancedInactive = new Gdk.Pixbuf(null, "iMetaGui.Images.advanced_inactive.png");
				return _TabAdvancedInactive;
			}
		}
		private static Gdk.Pixbuf _TabGeneral;
		public static Gdk.Pixbuf TabGeneral
		{
			get
			{
				if(_TabGeneral == null)			
					_TabGeneral = new Gdk.Pixbuf(null, "iMetaGui.Images.generalsettings.png");
				return _TabGeneral;
			}
		}
		private static Gdk.Pixbuf _TabGeneralInactive;
		public static Gdk.Pixbuf TabGeneralInactive
		{
			get
			{
				if(_TabGeneralInactive == null)			
					_TabGeneralInactive = new Gdk.Pixbuf(null, "iMetaGui.Images.generalsettings_inactive.png");
				return _TabGeneralInactive;
			}
		}
		private static Gdk.Pixbuf _TabMovies;
		public static Gdk.Pixbuf TabMovies
		{
			get
			{
				if(_TabMovies == null)			
					_TabMovies = new Gdk.Pixbuf(null, "iMetaGui.Images.moviesettings.png");
				return _TabMovies;
			}
		}
		private static Gdk.Pixbuf _TabMoviesInactive;
		public static Gdk.Pixbuf TabMoviesInactive
		{
			get
			{
				if(_TabMoviesInactive == null)			
					_TabMoviesInactive = new Gdk.Pixbuf(null, "iMetaGui.Images.moviesettings_inactive.png");
				return _TabMoviesInactive;
			}
		}
		private static Gdk.Pixbuf _TabTv;
		public static Gdk.Pixbuf TabTv
		{
			get
			{
				if(_TabTv == null)			
					_TabTv = new Gdk.Pixbuf(null, "iMetaGui.Images.tvsettings.png");
				return _TabTv;
			}
		}
		private static Gdk.Pixbuf _TabTvInactive;
		public static Gdk.Pixbuf TabTvInactive
		{
			get
			{
				if(_TabTvInactive == null)			
					_TabTvInactive = new Gdk.Pixbuf(null, "iMetaGui.Images.tvsettings_inactive.png");
				return _TabTvInactive;
			}
		}
		#endregion
	}
}

