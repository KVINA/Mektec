   M S B A M L   `   `   `  ����B  >AppMMCV, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;"clr-namespace:AppMMCV.View.SystemsAppMMCV.View.Systems  5   	   "    AppMMCV.View.Systems.LoginUC   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   4local"clr-namespace:AppMMCV.View.Systems     5   	   _ [MaterialDesignThemes.Wpf, Version=4.5.0.0, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5   	   -   5        .��5
      � $��1��6     � ColumnDefinitions  5   
   �� 5      6!   .+�5   
   �� $��/Image/Mektec.JPG��6   $
��Fill=�6+   $��1q�6:   � 5   
   $��Center=�6   $��Center=�6/   .+�5      ��  �� CornerRadius$ 20��6    � 	Resources 5         MyBrush �� Opacity �� Color��%9   &	        �� 5      $
 0.95Q�66   $ White��6E   5      ��5      �� 5       �� ResourceKey$ MyBrush��6%   ._�5      � $��Center=�6   $��20q�62   $	��300��6>    � RowDefinitions 5      �� 5      $
��Auto �6(   68   �� 5      $
��Auto �6(   68   �� 5      $
��Auto �6(   68   .+�5"      � $��Center=�6    5#       �� 
TargetType �� Property	 �� Value��%�   '��       �� 5$      " ��6$   .�5%   "   ��  � ��$
	 Bold"�6?   6N   �� 5&   "    � ��
	 ��  �6N   �� 5'   "    � ��$	 20(�6=   6J   5(      5)       5*      �� 5+      $
��Auto �6,   6<   �� 5,      $
��Auto �6,   6<   .+�5.      �� 
 J� Opacity$
 1Q�6    $��MEKTEC MANUFACTURING��6,   �� 5/      $��1��6    $��CORPORATION (VIETNAM)��6-   50      � 52      $��1��6    53      �� 54      $
��Auto �6,   6<   �� 55      $
��Auto �6,   6<   .+�57      ��  �� CharacterCasing$ Upper=�6"   $��16(�6:   $
��0 10q�6�     MaterialDesignFilledTextBox#��[ 6�   )  #MaterialDesignThemes.Wpf.HintAssist   Hint$ Username��6�   .  (MaterialDesignThemes.Wpf.TextFieldAssist   TextFieldCornerRadius$ 10��6�   ��6H   �� *
Username+ �� UpdateSourceTrigger$ PropertyChanged=�6�   G� 58      -   5        $��16(�58   &   $
��0 10q�6�   $��1��6�    # MaterialDesignFilledPasswordBox#��[ 6�   $ Password��6�   $ 10��6�   59      � 5;      $��2��6    5<      �� 5=      $
��Auto �6,   6<   �� 5>      $
��Auto �6,   6<   �� 5?      $
��Auto �6,   �� 5@      $
��Auto �6,   .+�5B      �� -   5        $��
ckb_status��5B   #   $��Remember password��65   $��Right=�6Q   $��Italic$�6m   �� 5D      -   5        $��1��5D   !     MaterialDesignRaisedButton#��[ 5E      3  -MaterialDesignThemes.Wpf.ButtonProgressAssist   Value$ -1Q�5F         IsIndicatorVisible .    IsIndeterminate . $
��0 10q�5I      $��40��6   $
��Bold"�6+   +  %MaterialDesignThemes.Wpf.ButtonAssist   CornerRadius$ 15��6=   $��Login��6k   �� 5J      $��2��6$   $��Right=�61   $��Italic$�6M   
����  � � Cursor$
 Hand��6r   B� 5K      .�6   Register an account6-   5L      �� 5M      $��3��6$   $
��Wrap=�61   $��Right=�6d   
���  ����6E   �� *statusMessage+5N      5O      5P      5Q      5R      5S      5T      