   M S B A M L   `   `   `  ����B  >AppMMCV, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:AppMMCV.View.HRMAppMMCV.View.HRM  5      1    +AppMMCV.View.HRM.Canteen_MealDistributionUC   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      0localclr-namespace:AppMMCV.View.HRM     5      _ [MaterialDesignThemes.Wpf, Version=4.5.0.0, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5        � 	Resources  5
       �� 
TargetType   Style_Textblock_Result �� Property �� Value �� Triggers P� Property P� Value   Style_Textblock_Notification��%�  &	        &	 )    �� 5   
   " ��6   .�5      ��  � ��$
 Wrap=�6-   �� 5       � ��$
 Bold"�6+    5      P� 5       � ��$ OK��6*   . �5      ��  � ��
 ��  �63   5      P� 5       � ��$ NG��6*   . �5      ��  � ��
 �  ��63   5      5      �� 5   
   " ��6   .�5      ��  � ��$
 Wrap=�6-   �� 5       � ��$
 Bold"�6+   �� 5       � ��
 ��  �6+   5      5      .��5      A�  A� Rows$ 2��6    A� Columns$ 2��6   .+�5    
   �� $��Số lượng lũy kế��6   $��2q�6,   .��5!      � 	 � ColumnDefinitions	 5"      �� 5#      �� 5$      $
��Auto �6'   �� 5%      .+�5'      �� $	��150(�67   $��Center=�6F   $��Center=�6a   $
��Bold"�6~   ��6   �� *	MealCount+6~   �� 5(      $��1��6   $��3��6/   $	��0 2q�69   $��Stretch=�6F   
 �� ResizeBehavior$
 PreviousAndNext=�6d   �� 5)      $��2��6   #��[  6,   $	��150(�6z   $��Center=�6�   $��Center=�6�   $
��Bold"�6�   ��6\   �� *ResultStatus+6�   �� 5*      $��3��6   $��30(�6M   $
��Bold"�6[   
������$��Center=�6�   ��60   �� *ClockString+6�   5+      5-      �� 5/   
   $@��:Mô tả chi tiết nếu  kết quả thẻ trả về NG��6   $��2q�6F   .��50      �� -   5        $��txt_notification��50       �� TextWrapping$
 Wrap=�51      $��50(�6*    �� 
IsReadOnly .   MaterialDesignTextBox#��[ 6J   ��50   .   �� *NotifyContent+��6M   �� *NotifyBrush+51   J   52      �� 55   
   $��Thông tin thẻ��6   $��2q�6*   .��56      � 	 57      �� 58      �� 59      $
��0.5* �6'   .+�5;      A� $ 1��6   $ 3��6*   .+�5<      �� -   5        $��
txt_cardID��5<       .  )  #MaterialDesignThemes.Wpf.HintAssist   Hint$ ID Thẻ��5=      $��25(�6F     MaterialDesignFilledTextBox#��[ 6T   $��2q�6�   �� 5>      -   5        $��txt_employeeCode��5>       . $ Mã nhân viên��6f   $��25(�6�   #��[ 6�   $��2q�6�   ��66   �� *EmployeeCode+6�   �� 5?      -   5        $��txt_employeeName��5?       . $ 	Họ tên��6f   $��25(�6�   #��[ 6�   $��2q�6�   ��66   �� *EmployeeName+6�   5@      �� 5B       �� CornerRadius$ 5��6   $��1q�6*   
�������$��1��6Q   ��5C      !� 5D       !� Visual 5E      �� 5F   "   $��UniformToFill=�6A   ��6(   �� *Avata+6A   5H      5J      5K      5L      �� 5M   
   $��Lịch sử quẹt thẻ��6   $��2q�6-     5N      *  $System.Windows.Controls.DataGridCell  MaterialDesignDataGridCell �� BasedOn �� 
TargetType��%�   '        7  �� 5O      "  6   8   .�5P      ��  � ��$ 16 0 0 0q�6/   �� 5Q       � �� 5R      �� 5S      "  6.   .R�5T   "   � #��z��6'   .+�5U   &   �� $��Center=�67   5V   #   5W      5Y      5Z      5[      .��5\      &   System.Windows.Controls.DataGrid     
IsReadOnly .    AutoGenerateColumns .  ��6   �� *EmployeeSwipeCards+   Columns 5]      4  .System.Windows.Controls.DataGridTemplateColumn  5^      ,  &System.Windows.Controls.DataGridColumn   Header$ Ảnh��6-      CellTemplate 5_      �� 5`      .R�5a   "   �� $��60��6A   $��60��6M   ��6(   �� *Avata+6M   5b      5d      0  *System.Windows.Controls.DataGridTextColumn  5f      $ Mã NV��6)   1  +System.Windows.Controls.DataGridBoundColumn   ElementStyle# [ 6Y      Binding 68   �� *EmployeeCode+6Y     5g      $ Tên NV��6)   # [ 6Z    69   �� *EmployeeName+6Z     5h      $ Kết quả��6)   # [  6U    6:   �� *Result+6U     5i      $ Thông báo��6)      MaxWidth$	 250Q�6]   # [ 6l    6<   �� *Notification+6l   5k      5l      5m      5n      