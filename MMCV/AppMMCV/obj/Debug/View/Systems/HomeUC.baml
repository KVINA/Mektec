   M S B A M L   `   `   `  ����B  >AppMMCV, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:AppMMCV.ServicesAppMMCV.Services  5   	   3clr-namespace:AppMMCV.ServicesAppMMCV.Services  5   	   !    AppMMCV.View.Systems.HomeUC   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   0localclr-namespace:AppMMCV.Services     5   	   _ [MaterialDesignThemes.Wpf, Version=4.5.0.0, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5   	   ,serviceclr-namespace:AppMMCV.Services  5   	   .��5      )  #MaterialDesignThemes.Wpf.DrawerHost  -   5        $��mdh_menu��5           LeftDrawerContent  5   
   �� 5      $	��300��6    � 	Resources 5       �� MergedDictionaries �� Source   PrimaryHueLightBrush  
Primary400 �� Color�� 5       5      �� 5      $i cpack://application:,,,/MaterialDesignColors;component/Themes/MaterialDesignColor.Green.Primary.xaml?�61   %&   &	        7  �� 5      8   6H   5      ��5      � 5      $��52��6    � ColumnDefinitions 5      �� 5      �� 5      $
��Auto �6/   .+�5      ��  �� Orientation$ 
Horizontal=�6%   .+�5      �� $��Center=�6;   $
��Left=�6V   $��0q�6q    � Cursor$
 Hand��6�   ��6%   Z� ��5   "   �� 5    &    �� 
TargetType" ��6,   .�5!   *   �� 	 �� Property	 � ��
 �� Value

 �����6G    �� Triggers 5"   *   P� 5#   .    P� Property �@J�IsMouseOver P� Value . . �5$   2   �� 	 � ��

 � � �6O   5%   /   5'   '   .��5)   "   '  !MaterialDesignThemes.Wpf.PackIcon     Kind$
 Home=�6:   $��Center=�6F   $��26��6a   $��26��6l   5*      �� 5+      $��Center=�6(   $
��Home��6C    $  MaterialDesignHeadline6TextBlock#��[ 6O   
�������$��5q�6�   5,      d� 5.       ' #MaterialDesignHamburgerToggleButton#��[ 6'   $��1��6d   $��Center=�6t   
�������$
 Hand��5/   '      CloseDrawerCommand#��Z 6w    d� 	IsChecked 65   ��  �� Path$ IsLeftDrawerOpen� �� ElementName$ mdh_menu��6w   50      .��52      �� -   5        $��itc_MainMenu��52      $��5q�6U   $��1��6`   ��63   �� *	MenuInfos+��53      �� 54      .R�55      �� 
�������$��0 0 0.5 0.5q�68   ._�56   "   9� -   5        $��1q�56   +    9� ExpandDirection$
 Down=�66   
 � Tag 6j   �� *
Subject_id+ 9� 
IsExpanded 6�   �� *IsOpen+��57   &   � 58   *   .+�59   .     $��30��6d   $��30��6o    6F   �� *Subject_icon+6o   �� 5:   .   $��14(�6V   $
��Bold"�6d   $��Center=�6v   ��68   �� *Subject_name+6v   5;   +   .��5>   &   �� $��	40 0 20 0q�6R   ��6.   �� *
ChildsInfo+��5?   *   �� 5@   .   " ��64    ' #MaterialDesignNavigationListBoxItem �� BasedOn# [ 6M   .�5A   2   �� 	 � ��$
 5q�6K   �� 5B   2   	 � ��$
 10q�6L   �� 5C   2   	 �@�Tag
 6H   �� *	Menu_id+�� 5D   2   	 �@�ToolTip
 6L   �� *Menu_description+�� 5E   2   	 �@�Cursor$

 Hand��6K   -   5        5G   /   ��5I   *   �� 5J   .   .R�5K   2   �� $
 Hand��6W   ��6<   �� *	Menu_name+6W   5L   /   5N   '   5P   #   5Q      5S      ��5V      �� 5W      .R�5X      �� 5Y      5[      5\      .��5^   
   �  � RowDefinitions 5_      �� 5`      $
��Auto �6    �� 5a      .+�5c      (  "MaterialDesignThemes.Wpf.ColorZone     Mode$ PrimaryDark=�6'   .��5d      � $��5 q�6    5e      �� 5f      $
��Auto �6+   �� 5g      �� 5h      $
��Auto �6+   .+�5j      d� 
�������#��[ 66   $
 Hand��5k         OpenDrawerCommand#��Z 6o    6-   �� $ IsLeftDrawerOpen�$ mdh_menu��6o   � 5l      $��1��6    $��Center=�60   $
��Left=�6K    5m      ��%�   '��       7  �� 5n      " ��6$   8   .�5o   "   �� 	 � ��$
 5q�6;   �� 5p   "   	 � ��$

 Bold"�6?   �� 5q   "   	 � ��

 �����6?   5r      5t      .+�5u      �� -   5         ��Name$ 
txt_header��5u   $   $��MMCV SYSTEM��68   5v      �� 5x      $��2��6!   $��Center=�61   $��5q�6L   $ 
Horizontal=�6W   $��Right=�6p    5y       7 3MaterialDesignFloatingActionMiniSecondaryDarkButton��%4   '��       7  �� 5z      " ��6$   8   5|      5}      .+�5�      '  !MaterialDesignThemes.Wpf.Snackbar  -   5          Name$ SnackbarInfo��5�   2      MessageQueue 6I   4  .MaterialDesignThemes.Wpf.MessageQueueExtension     Message. 5�      .  (MaterialDesignThemes.Wpf.SnackbarMessage  .��5�   "   �� $�� ��6,   5�      5�      o� 5�      $��0��6   $��18��6)    o� 
IsMainMenu .  .@�5�      m� -   5        $��mni_user��5�   '   4  .System.Windows.Automation.AutomationProperties   IsOffscreenBehavior$ Onscreen=�67   .@�5�   "   m� $
 Hand��6�   $
��15 0q�6�   ��6+   �� "  AppMMCV.Services.DataService    UserInfo! �� Source#! Z  $ username�" �� Mode$" TwoWay=�# m� Icon# 6�   0	  *MaterialDesignThemes.Wpf.PackIconExtension	  $ 	  Kind$$ AccountOutline=�m� 5�   "   -   5        $��IsAdmin��5�   +   $
 Hand��6:   $
��15 0q�6H   $��Administrator��6V   # 6m   	  $$ AdministratorOutline=�m� 5�   "   -	   5        $
 Hand��5�   +   $
��15 0q�69   $��Change Password��6G   # 6�   	  $$ PasswordCheckOutline=�m� 5�   "   $
 Hand��6+   $
��15 0q�69   $��Log Out��6G   # 6X   	  $$ Logout=�5�      5�      %
  MaterialDesignThemes.Wpf.Badged
  5�      $��Center=�60   $��5q�6K   % 
  Badge$% 0��6V   .��5�      �� -
   5        $��btn_shownotify��5�   %   .��5�   "     $ Notifications=�6:   5�      5�      
  5�      $��Center=�60   $��5q�6K   $% 0��6V   .��5�      �� -   5        $��btn_showmessage��5�   %   .��5�   "     $ Message=�6:   5�      5�      �� 5�      -   5        $��5q�5�   !   .��5�        $ Account=�66   5�      5�      5�      5�      #  MaterialDesignThemes.Wpf.Card  5�      -   5        $��card_renderbody��5�   /   $��1��6"   6/   � 5�      $��1��6   ��5�      �� 5�      " �6   .�5�      �� 	 � y�$
 Hidden=�67    5�      �� 5�      & �� Binding& 6*   �� $ card_renderbody��$ Content�' �� Value' 6h   Z� .a�5�   "   �� 	 � y�$
 Visible=�6?   5�      5�      .+�5�      �� ��5�      �� 5�      ( �� ImageSource$( /Image/BkHome.png��6%   .+�5�      �� $��Hệ Thống Quản Lý��6    $��80(�68   $
��Wrap=�6F   
����  �$
��Bold"�6l   $��Center=�6~   $��Center=�6�   �� 5�      $��Phòng Hành Chính��6    $��80(�68   $
��Wrap=�6F   
����  �$
��Bold"�6l   $��Center=�6~   $��Center=�6�   5�      5�      5�      5�      5�      