   M S B A M L   `   `   `  ����B  >AppMMCV, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null7 clr-namespace:AppMMCV.View.AdminAppMMCV.View.Admin  5      %    AppMMCV.View.Admin.MenuSettings   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      2local clr-namespace:AppMMCV.View.Admin     5      _ [MaterialDesignThemes.Wpf, Version=4.5.0.0, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDisign1http://materialdesigninxaml.net/winfx/xaml/themes  5        � 	Resources  5
       �� 
TargetType   	style_btn��%+   &	        �� 5   
   " ��6   5      5      .��5      )  #MaterialDesignThemes.Wpf.DrawerHost     IsRightDrawerOpen 6    �� *IsOpenRightDrawer+   RightDrawerContent 5   
   �� 5      ��6   �� *HeaderActive+��67   �� *UsercontrolActive+.��5   
   �  � RowDefinitions 5      �� 5      �� 5      .+�5      �  � ColumnDefinitions 5      �� 5      �� 5      .+�5      �� $��2q�6   ��5       � 5!       5"      �� 5#   "   �� 5$   "   $
��Auto �63   .+�5&      �� $��Subject��6(   $
��Bold"�67   
�������6I   �� 5'      -   5        $��1��5'   %   $��18��65   
 � Tag$ Add Subject��6W    � ToolTip$	 Add��5(   [   ��6%   0  *MaterialDesignThemes.Wpf.PackIconExtension     Kind$	 Add=�	   Size$	 11X�6[   5*      .��5-      &   System.Windows.Controls.DataGrid  -   5        $��dtg_subject��5-      
   
IsReadOnly
 . $��2q�6D      AutoGenerateColumns .   �� SelectedItem 6O   �� *SelectedSubject+��5.   ;   �� *DataSubject+   Columns 5/      4  .System.Windows.Controls.DataGridTemplateColumn  50         CellTemplate 51   "   �� 52   &   .R�53   *   � .+�54   .   �� -   5        $ Edit Subject��54   L   $��0 0 2 0q�6`   $��18��55   h   ��61     $
 Edit=�$	 11X�6h   �� 56   .   -   5        $��1��56   5   $��18��6E   $ Add Menu��6g   $��2 0 0 0q�6v   $ Add menu��57   g   ��61     $	 Add=�$	 11X�6g   58   +   59   '   5;      0  *System.Windows.Controls.DataGridTextColumn  5<      ,  &System.Windows.Controls.DataGridColumn   Header$ Id��61   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6=   �� *
Subject_id+  5=      $
 Name��61    6?   �� *Subject_name+  5>      $
 Icon��61    6?   �� *Subject_icon+  5?      $
 Time��61    6?   �� *	Create_at+  5@      $ Username��61    6C   �� *	Create_by+5B      5C      �� 5E      $��2q�6   $��1��6&   ��5F      �� 5G      $
��Menu��6$   $
��Bold"�60   
�������6B   .��5I        -   5        $��dtg_menu��5I      
 . $��2q�5K   @    5J      �� *SelectedMenu+��5K      �� *
DataMenu+ 5L        5M       5N   "   �� 5O   &   .R�5P   *   � .+�5Q   .   �� -   5        $ 	Edit Menu��5Q   L   $��0 0 2 0q�6]   $��18��5R   h   ��61     $
 Edit=�$	 11X�6h   �� 5T   .   -   5        $��1��5T   5   $��18��6E   $ Add Item��6g   $��2 0 0 0q�6v   $ Add menu item��5U   g   ��61     $	 Add=�$	 11X�6g   5V   +   5W   '   5Y      5[      5\      5]      �� 5_      $��2q�6   $��1��6"   ��5`      �� 5a      $��	Menu item��6    $
��Bold"�61   
�������6C   .��5c        -   5        $��dtg_menuItem��5c      $��2��6T   
 . $��2q�6s   ��6/   �� *DataMenuItem+ 5d        5e       5f      �� 5g   "   .R�5h   &   � .+�5i   *   �� -	   5        $ 	Edit Item��5i   H   $��18��5j   h   ��61     $
 Edit=�$	 11X�6h   5k   '   5l   #   5n      5p      5q      5r      5s      5u      