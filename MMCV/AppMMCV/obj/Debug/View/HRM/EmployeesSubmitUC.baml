   M S B A M L   `   `   `  ����B  >AppMMCV, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:AppMMCV.View.HRMAppMMCV.View.HRM  5      (    "AppMMCV.View.HRM.EmployeesSubmitUC   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      0localclr-namespace:AppMMCV.View.HRM     5      _ [MaterialDesignThemes.Wpf, Version=4.5.0.0, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5        � 	Resources  5	       �� 
TargetType !  MaterialDesignOutlinedTextBox �� BasedOn �� Property �� Value " MaterialDesignOutlinedComboBox(  "System.Windows.Controls.DatePicker $  MaterialDesignOutlinedDatePicker��%}  '��       7   '�� q     7  '  �     7  �� 5
   
   " ��6   8   .�5      ��  � ��$ 5q�6'   �� 5       � ��$	 150��6&   5      �� 5   
   " ��6   8   .�5      ��  � ��$ 5q�6'   �� 5       � ��$	 150��6&   5      �� 5   
   "  6   8   .�5      ��  � ��$ 5q�6'   �� 5       � ��$	 150��6&   5      5      .��5      �� $��
Form Input��6   .��5   
   � -   5        $��	grid_body��5      $��Center=�6     � RowDefinitions 5      �� 5       �� 5!      �� 5"      �� 5#      �� 5$      �� 5%      �� 5&      �� 5'       � ColumnDefinitions 5*      �� 5+      �� 5,      .+�50      �� -   5        $��txt_full_name��50   &   
 � Tag$ 	full_name��6   $��2��6;   $
��Auto��6O   )  #MaterialDesignThemes.Wpf.HintAssist   Hint$ 	Full name��6\   �� 52      -   5        $��txt_code��52   *   $ employee_code��6   $��1��6:   $ ID code��6G     53      -   5        $��txt_dateHire��53   )   $ 	hire_date��6   $��1��6=   $��1��6J   $ Date of  hire��6Z     55      -   5        $��txt_dateMaternity��55   4   $ maternity_leave_date��6   $��2��6M   $ Date of Maternity��6Z     56      -   5        $��txt_dateResign��56   0   $ resignation_date��6   $��2��6F   $��1��6S   $ Date of Resign��6c   �� 58      -   5        $��cbb_department��58   (   $ 
department��6   $��3��6>   $��2��6K   $
��Auto��6_   	 �� 
IsEditable	 . $ 
department��6~   �� 5:      -   5        $��cbb_section��5:   %   $ section��6   	 . $��4��6J   $��2��6W   $
��Auto��6k   $ section��6x   �� 5<      -	   5        $��cbb_position��5<   &   $ position��6   	 . $��5��6L   $��2��6Y   $
��Auto��6m   $ position��6z   �� 5>      -
   5        $��cbb_cost_center��5>   )   $ cost_center��6   	 . $��6��6R   $ Cost center��6_   �� 5?      -   5        $��
cbb_status��5?   $   $ status��6   $��6��66   $��1��6C   $ status��6S   .@�5@      �� �� 5A      $��0: Nghỉ việc��6   �� 5B      $��1: Đang làm��6   �� 5C      $��2: Thai sản��6   5D      �� 5F      -   5        $��
btn_submit��5F   *   $��7��6<   $��5q�6I   $��Submit��6T   �� 5G      )  #MaterialDesignThemes.Wpf.DrawerHost
   CloseDrawerCommand#��Z
 6   $��7��6W   $��1��6d   $��5q�6t   $��Close��6   5H      5I      5J      