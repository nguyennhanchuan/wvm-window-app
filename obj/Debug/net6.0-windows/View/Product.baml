   M S B A M L   `   `   `  ����I  EVendingMachine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null9!clr-namespace:VendingMachine.ViewVendingMachine.View  5      =#clr-namespace:VendingMachine.HelperVendingMachine.Helper  5      !    VendingMachine.View.Product   5      W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      c _MahApps.Metro.IconPacks.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=0c0d510f9915137ag cMahApps.Metro.IconPacks.Material, Version=4.0.0.0, Culture=neutral, PublicKeyToken=0c0d510f9915137a:Icon-http://metro.mahapps.com/winfx/xaml/iconpacks   5      /local!clr-namespace:VendingMachine.View   5      .cv#clr-namespace:VendingMachine.Helper   5      -   5          >�Name$  	UcProduct��5
      .��5      �� -   5         ��Name$ ProductBorder��5      $��0q�6$    �� CornerRadius$ 10��68   $	��160��6J   $	��200��6V   $	��Top=�6c   $��
0 20 20 20q�6{    J� Effect 5   
   3  -System.Windows.Media.Effects.DropShadowEffect  5         
BlurRadius$ 10Q�6      ShadowDepth$ 2Q�6/      Opacity$	 0.2Q�6?      	Direction$	 270Q�6M   ��5   
   �� 5       �� 
TargetType" ��6   .�5      �� 	 �� Property	 � ��
 �� Value

 �����6/    �� Triggers 5      P� 5       P� Property �@J�	IsFocused P� Value$
 True��63   . �5      �� 	 � ��
 �#55E6C167   5      5!      ._�5$   
   �  � RowDefinitions 5%      �� 5&      $��5* �6    �� 5'      $��* �6    .+�5*      �� 
�������$ 10��6(   
�������$��1q�6N   ._�5+      �  5,      �� 5-      $
��auto �6(   �� 5.      $��* �6(   .+�51      �� -   5        $ DiscountBorder��51      $��0��65   ��52      �� 53      " ��6$    54   "   �� 55   &    �� Binding 62   ��  �� ElementName$ 	UcProduct�� �� Path$ ProductDiscount�+   %VendingMachine.Helper.IsNullConverter   Instance �� 	Converter# Z  �� ConverterParameter Z�  �� Value	 False6�   .a�56   *   �� 	 � ��
 �#ecf0f16G   �� 57   *   	 �@��CornerRadius$
 10��6I   �� 58   *   	 � ��$
 40��6B   �� 59   *   	 � ��$
 40��6C   �� 5:   *   	 � ��$
 Right=�6P   �� 5;   *   	 � ��$
 	0 10 10 0q�6C   5<   '   5>      ._�5@      �� $��15(�6a   $��SemiBold"�6o   $��Center=�6�   $��Center=�6�   ��6$   �� $ 	UcProduct��$ ProductDiscount�6�   5A      �� 5C      $��1��6   ��6)   �� $ 	UcProduct��$ ProductImage�5D      5E      �� 5G      $��1��6   
���   �$��15(�6r   $��Center=�6�   $��Center=�6�   $��SemiBold"�6�   ��6%   �� $ 	UcProduct��$ ProductPrice�6�   5I      5K      5L      