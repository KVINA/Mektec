   M S B A M L   `   `   `  ����B  >AppMMCV, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:AppMMCV.View.HRMAppMMCV.View.HRM  5      4    .AppMMCV.View.HRM.Canteen_MasterRegisterMealsUC   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      0localclr-namespace:AppMMCV.View.HRM     5      _ [MaterialDesignThemes.Wpf, Version=4.5.0.0, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5        � 	Resources  5       �� 
TargetType   MaterialDesignRaisedButton �� BasedOn �� Triggers �� Binding �� Value �� Property �� Value��%�   '��       7   �� 5   
   " ��6   8    5      �� 5       6   �� *	IsLoading+ True6<   .a�5      ��  � {� .  62   5      5      5      .��5      �  � ColumnDefinitions 5   
   �� 5      $
��Auto �6   �� 5      .+�5   
   �� $
��Auto=�6   $
��Auto=�6:   .��5      R� -   5        
�������$��5q�5   [   ��6.   �� *DataCompanyStructure+��5      �� 5      	 �� ItemsSource	 6/   �� *
Children+.R�5       �� 
 �� Orientation$
 
Horizontal=�6%   .+�5!      '  !MaterialDesignThemes.Wpf.PackIcon     Kind$ FolderAccountOutline=�66   $��20��6R   $��20��6]   $��Center=�6i   �� 5"      ��6(   �� *Name+5#      5$      ��5&      �� 5'      " Q�6   .�5(      ��  �@Q�
IsSelected 67   �� *
IsSelected+�� 5)       �@Q�
IsExpanded 67   �� *
IsExpanded+�� 5*       �@�Cursor$
 Hand��63   �� 5+       � ��$ 16(�65   �� 5,       � ��$ 2q�63   5-        5/        MaterialDesignTreeView��%4   'R�       7  �� 50      " R�6   8   52      53      54      56      � 59   
   $��1��6    � RowDefinitions 5:      �� 5;      �� 5<      $
��Auto �6    .+�5?      � $��1��6   $��Right=�6%   .+�5@      �� $��Tải danh sách��6   $��5q�61   ��6<   �� *Command_ExecuteDownloadMaster+5A      )  #MaterialDesignThemes.Wpf.DialogHost  5C         IsOpen 6(   �� *	IsLoading+   DialogContent 5D      �� 5E      .+�5F      �� $��Processing ...��6$   $	��5 2q�6:   #  MaterialDesignThemes.Wpf.Card  5G      $��5q�6.      UniformCornerRadius$ 14Q�69   $��4q�6R   $��Center=�6^   .��5H      �  % !MaterialDesignCircularProgressBar#��[ 6*    � Value$ 50Q�6e    � IsIndeterminate . 6p   5I      5J      .��5M      &   System.Windows.Controls.DataGrid     
IsReadOnly . $��2q�6.      AutoGenerateColumns .  ��69   �� *DataEmployees+   Columns 5N      0  *System.Windows.Controls.DataGridTextColumn  5O      ,  &System.Windows.Controls.DataGridColumn   Header$ ID��6-   1  +System.Windows.Controls.DataGridBoundColumn   Binding 69   �� *ID+  5P      $ Employee code��6-    6D   �� *EmployeeCode+  5Q      $ Employee name��6-    6D   �� *
FullName+  5R      $ Position��6-    6?   �� *
Position+  5S      $ Location��6-    6?   �� *
Department+5U      5V      5W      5X      5Y      