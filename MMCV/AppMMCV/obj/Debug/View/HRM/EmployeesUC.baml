   M S B A M L   `   `   `  ����B  >AppMMCV, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:AppMMCV.View.HRMAppMMCV.View.HRM  5      "    AppMMCV.View.HRM.EmployeesUC   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      0localclr-namespace:AppMMCV.View.HRM     5      _ [MaterialDesignThemes.Wpf, Version=4.5.0.0, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5        � 	Resources  5
       �� 
TargetType !  MaterialDesignOutlinedTextBox �� BasedOn �� Property �� Value��%   '��       7   �� 5   
   " ��6   8   .�5      ��  � ��$	 150��6&   �� 5       � ��$ 5q�6'   5      5      .��5      )  #MaterialDesignThemes.Wpf.DrawerHost  -   5        $��drh_main��5          RightDrawerContent 5   
   (   "AppMMCV.View.HRM.EmployeesSubmitUC  5      -   5          Name$ 	uc_submit��5   &   .��5   
   �  � RowDefinitions 5      �� 5      $
��Auto �6    �� 5      .+�5      �� ��6   Z� ��5      �� 5      $��Chức năng��6    $
��Bold"�61   
����  � �� Padding$ 5q�6U   .��5       � -   5        $��wpn_inputsearch��5       .+�5!      �� -   5        $��5q�5!   2   .��5"      �� 	 �� Orientation$	 
Horizontal=�6%   .+�5#      '  !MaterialDesignThemes.Wpf.PackIcon  
   Kind$
 Search=�66   $��11(�6D   $��Center=�6R   �� 5$      $��Tìm kiếm��6(   $ 2q�68   5%      5&      �� 5(      -   5        $��5q�5(   8   .��5)      �� $	 
Horizontal=�6%   .+�5*        $

 Plus=�66   $��11(�6B   $��Center=�6P   �� 5+      $��Thêm employees��6(   $ 2q�6>   5,      5-      �� 5/      -   5        $��5q�5/   7   .��50      �� $	 
Horizontal=�6%   .+�51        $
 
FileExport=�66   $��11(�6H   $��Center=�6V   �� 52      $��Xuất file đăng ký��6(   $ 2q�6A   53      54      5D      5E      &   System.Windows.Controls.DataGrid  5G      -   5        $��dtg_employees��5G   $   $��1��6      
IsReadOnly . $��2q�6�   ��69   ��  �� ElementName$ 	uc_submit�� �� Path$ 	Transport�   Columns 5H      4  .System.Windows.Controls.DataGridTemplateColumn  5I         CellTemplate 5J      �� 5K      .R�5L   "   �� $	 
Horizontal=�6-   .+�5M   &   �� -   5         � ToolTip$
 Copy��5M   -   $��18��6�   $	��2 0q�6�   ��6X   0  *MaterialDesignThemes.Wpf.PackIconExtension     Kind$ ContentCopy=�   Size$ 11X�6�   �� 5N   &   -	   5        $
 Edit��5N   -   $��18��6�   $	��2 0q�6�   ��6X     $
 Edit=�$ 11X�6�   5O   #   5P      5R      5T      5U      5V      5W      