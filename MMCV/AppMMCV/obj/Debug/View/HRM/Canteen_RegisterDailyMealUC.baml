   M S B A M L   `   `   `  ����B  >AppMMCV, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:AppMMCV.View.HRMAppMMCV.View.HRM  5      2    ,AppMMCV.View.HRM.Canteen_RegisterDailyMealUC   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      0localclr-namespace:AppMMCV.View.HRM     5      _ [MaterialDesignThemes.Wpf, Version=4.5.0.0, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5      .��5
      )  #MaterialDesignThemes.Wpf.DialogHost      IsOpen  6    �� *	IsLoading+   DialogContent 5   
   �� 5      .+�5      �� $��Processing ...��6   $��5q�62   � 5       %  !MaterialDesignCircularProgressBar#��[  6    � Value$ 50Q�6Y    � IsIndeterminate . $��Center=�6{   $��5q�6�   5      .��5   
   � $��1��6    � RowDefinitions 5      �� 5      $
��Auto �6    �� 5      $
��Auto �6    �� 5      �� 5      $
��Auto �6    .+�5      � $��0��6   .+�5      (  "System.Windows.Controls.DatePicker  )  #MaterialDesignThemes.Wpf.HintAssist   Hint$
 Date��6C   $	��150��6i   $	��2 5q�6u    $  MaterialDesignOutlinedDatePicker#��[ 6�      SelectedDate 6   �� *DateRegister+6�   �� 5       �� DisplayMemberPath$
 Meal��6@   $ 	Meal time��6Y   $	��150��5   A   $	��2 5q�6M    " MaterialDesignOutlinedComboBox#��[ 6Z   ��5      �� *DataMealTime+ �� SelectedItem 5      �� *SelectedMealTime+6Z   �� 5      $��Clear attachment��6P   $��2q�6k   ��6   �� *$"Command_ExecuteClearAttachmentPath+6k   �� 5      $��Register file attach��6J   $��2q�6i   ��6   �� *Command_ExecuteRegisterMeals+6i   5      �� 5!      $��1��6   $��1q�6"   
�������	 �� CornerRadius$	 4��6I   
 �� Padding$
 2q�6Z   $��2q�6f   ._�5"      � $��2q�6    � ColumnDefinitions 5#      �� 5$      $
��Auto �6+   �� 5%      $
��Auto �6+   �� 5&      .+�5(      �� $
��Bold"�6   $��Attach file��6d     MaterialDesignFlatButton#��[ 6z   ��6/   �� *" Command_ExecuteGetAttachmentPath+6z   �� 5)      $��1��6   
�������$��1��6?   $	��5 0q�6I   $��Stretch=�6V   �� 5*      $��2��6    $	��5 0q�60   $��Center=�6=   ��6X   �� *AttachmentPath+5+      5,      � 5.      $��2��6    5/      �� 50      �� 51      $
��Auto �6'   .+�53      &   System.Windows.Controls.DataGrid     
IsReadOnly . $��2��6W   $��2q�6d      AutoGenerateColumns .  ��6   �� *DataRegisterDaily+   Columns 54      0  *System.Windows.Controls.DataGridTextColumn  55      ,  &System.Windows.Controls.DataGridColumn   Header$ Id��6-   1  +System.Windows.Controls.DataGridBoundColumn   Binding 69   �� *Id+  56      $ Ngày��6-    6;   �� *	Meal_date+ �� StringFormat$ 
yyyy-MM-dd��  57      $ Mã nhân viên��6-    6C   �� *Employee_code+  58      $ Tên nhân viên��6-    6D   �� *	Full_name+  59      $ Ca ăn��6-    6<   �� *Meal+  5:      $ 
Suất ăn��6-    6>   �� *Category_name+5<      �� 5>      $��1��6   $��Total��6<   ��6+   Z� .��5?      �� $��2q�6!   .+�5@      ��  �� 
IsReadOnly . $
 Cơm��64   $	��Top=�6u    ! MaterialDesignOutlinedTextBox#��[ 6�   $	��2 5q�6�   $	��100��6�   ��6Y   �� *
Total_rice+6�   �� 5B       . $ BMS 1:1��64   $	��Top=�6y   #��[ 6�   $	��2 5q�6�   $	��100��6�   ��6]   �� *
Total_bms1+6�   �� 5D       . $ BMS 2:2��64   $	��Top=�6y   #��[ 6�   $	��2 5q�6�   $	��100��6�   ��6]   �� *
Total_bms2+6�   5E      5G      5H      5K      5L      5O      