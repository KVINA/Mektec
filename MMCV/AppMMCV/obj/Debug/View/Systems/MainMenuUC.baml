   M S B A M L   `   `   `  ����B  >AppMMCV, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;"clr-namespace:AppMMCV.View.SystemsAppMMCV.View.Systems  5      %    AppMMCV.View.Systems.MainMenuUC   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      4local"clr-namespace:AppMMCV.View.Systems     5      _ [Microsoft.Xaml.Behaviors, Version=1.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a3i+http://schemas.microsoft.com/xaml/behaviors  5      _ [MaterialDesignThemes.Wpf, Version=4.5.0.0, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5        � 	Resources  5
      ��%    5      .��5      �   5   
    �� 
TargetType �� BasedOn �� Property �� Value �� Triggers P� Property P� Value��%�  'm�       'o� �    '�� K    '��     �� 5      " m�6    6*   Z� .�5      -   5        �� 5       �@�Cursor$
 Hand��6+   �� 5       � ��$ 14(�6-   �� 5       � ��$ Segoe UI)�6/   �� 5       � ��$
 Bold"�6/   �� 5       � ��$ UltraExpanded&�60   �� 5       � ��$ 28��6+    5      P� 5       �@m�Icon 6.   Z� . �5      ��  � ��$ 	0.5 0 0 0q�6<   �� 5       � ��
 �����68   5      5      �� 5!      " o�6    6&   Z� .�5"      ��  � ��$ 2q�6+   �� 5#       � ��
 �����6/   5$      �� 5&      " ��6    6,   Z� .�5'      ��  �@��TabStripPlacement$ Bottom=�66   �� 5(       � ��$ 1 -9 1 1q�6,   �� 5)       � ��$	 2 0q�6+   5*      �� 5,      " ��6    6)   Z� .�5-      ��  � ��$
 -2 0q�6+    5.      P� 5/       �@��
IsSelected . . �50      ��  � ��$
 -2 0q�63   51      53      55       � RowDefinitions 56   
   �� 57      $
��Auto �6   �� 58      .+�5;   
   � 	 � ColumnDefinitions	 5<      �� 5=      .+�5@      o� -   5        $��	menu_main��5@      �� 5B      $��1��6   $
��Bold"�5E      $��2q�6&   ��5B   $   �� *TitleInfo.Content+
 �� UpdateSourceTrigger$
 PropertyChanged=���5C      �� *TitleInfo.Background+$
 PropertyChanged=���5D      �� *TitleInfo.Foreground+$
 PropertyChanged=�5E   &   5F      �� 5I   
   -   5        $��tab_control��5I   "   $��1��6   6"   �� 5J   
   $��1��6   $��/Image/background.png��6   $��UniformToFill=�6<   $��2q�6T   ��5K      �� 5L      " ��6   .�5M      ��  � y�$ Hidden=�63    5N      �� 5O       �� Binding 6&   ��  �� ElementName$ tab_control�� �� Path$ Items.Count� �� Value 06d   .a�5P      ��  � y�$ Visible=�6;   5Q      5S      5U      5V      5W      