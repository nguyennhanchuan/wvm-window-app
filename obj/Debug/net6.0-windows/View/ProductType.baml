   M S B A M L   `   `   `  ����I  EVendingMachine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null9!clr-namespace:VendingMachine.ViewVendingMachine.View  5      %    VendingMachine.View.ProductType   5      W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      c _MahApps.Metro.IconPacks.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=0c0d510f9915137ag cMahApps.Metro.IconPacks.Material, Version=4.0.0.0, Culture=neutral, PublicKeyToken=0c0d510f9915137a:Icon-http://metro.mahapps.com/winfx/xaml/iconpacks   5      /local!clr-namespace:VendingMachine.View   5      -   5        $��ucProductType��5	      .��5      �� -   5          ��Name$  ProductTypeButton��5      
������ $��74��6A   .��5   
   ��  �� 
TargetType" ��6   .�5      ��  �� Property � �� �� Value
 �   �6+    �� Triggers 5      P� 5       P� Property �@J�	IsFocused P� Value$
 True��6/   . �5      ��  � ��
 �����63   5      5      ��5   
   �� 5       �� 
TargetType" ��6   .R�5      �� $��BorderTemplate��6'    �� Padding$	 5 5q�6   	 J� Effect	 5      3  -System.Windows.Media.Effects.DropShadowEffect  5      
   
BlurRadius$
 20Q�6+      ShadowDepth$ 1Q�6;      Opacity$	 0.1Q�6K      	Direction$	 270Q�6Y   ��5      �� 5      " ��6    .�5      ��  � ��
 �����6;   �� 5        � ��
 �����6<   �� 5!       � ��$ 0q�6@   �� 5"       �@��CornerRadius$ 25��6=   �� 5#       �@��Padding$ 50q�68   �� 5$       � ��$
 auto��66   �� 5%       � ��$	 200��69   �� 5&       � ��$ 50��67   �� 5'       � ��$ 5 0 5 0q�67   �� 5(       �@�FocusVisualStyle 6A   Z� 5)      ._�5,      �  � ColumnDefinitions 5-      �� 5.      $
��auto �6/   �� 5/      $��* �6/   .+�53      A� $	��0.2��6"   $��38��68   $��38��6C   $��0 0 10 0q�6O   $��Center=�6a   ��54      �� 55   "   StrokeColor�� �� Color$ #55E6C1��6G   ��58      �� 59   "    �� ImageSource 6-   ��  �� ElementName$ ucProductType�� �� Path$ Image�5;      �� 5>      $��1��6$   $��13(�6k   $��SemiBold"�6y   $
��Left=�6�   $��Center=�6�   ��64   �� $ ucProductType��$ Title�6�   5?      5@       �� Triggers 5B      P� 5C       �@J�	IsFocused$
 True��63   . �5D      ��  �� 
TargetName$ BorderTemplate��6!    � �� �#55E6C16S   5N      �� 5O       �� Value$
 True��6_    �� Binding 6"   �� $ ucProductType��$ IsActive�.a�5P      �� $ BorderTemplate��6!    � �� �#55E6C16S   5Z      5]      5_      5b      