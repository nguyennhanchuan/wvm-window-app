   M S B A M L   `   `   `  ����I  EVendingMachine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null9!clr-namespace:VendingMachine.ViewVendingMachine.View  5      "    VendingMachine.View.CartItem   5      W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      3local!clr-namespace:VendingMachine.View     5      c _MahApps.Metro.IconPacks.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=0c0d510f9915137ag cMahApps.Metro.IconPacks.Material, Version=4.0.0.0, Culture=neutral, PublicKeyToken=0c0d510f9915137a:Icon-http://metro.mahapps.com/winfx/xaml/iconpacks   5      -   5        $��
UcCartItem��5	      .��5
      ��   J� Effect  5   
   3  -System.Windows.Media.Effects.DropShadowEffect  5         
BlurRadius$ 20Q�6      ShadowDepth$ 1Q�6/      Opacity$	 0.1Q�6?      	Direction$	 270Q�6M   ��5   
   �� 5       �� 
TargetType" ��6   .�5      ��  �� Property � �� �� Value
 �����6/   �� 5       � ��$ 5 0 5 10q�6+   �� 5       �@��Padding$ 15 10q�6,   �� 5       �@��CornerRadius$ 20��61   �� 5       � ��$ 1q�64   �� 5       � ��
 �����60    �� Triggers 5      P� 5      	 P� Property	 �@J�IsMouseOver
 P� Value$

 True��65   . �5      ��  � �� �#f3f3f367   5      �� 5       �� Binding 6"   ��  �� ElementName$ 
UcCartItem�� �� Path$ IsActive� �� Value True6\   .a�5      ��  � �� �#f3f3f367   5      5       ._�5#   
   �  � ColumnDefinitions 5$      �� 5%      $
��auto �6#   �� 5&      $	��140 �6#   �� 5'      $��* �6#   .+�5+      A� $��50��6   $��50��6!   $��0 0 0 0q�6-   $	��Top=�6>   ��5,      �� 5-       �� ImageSource 6!   �� $ 
UcCartItem��$ 	ItemImage�5/      �� 52      $��1��6   $��12 0 0 0q�6)   .+�53      �� 
���   �$��13(�6f   $��SemiBold"�6t   ��6   �� $ 
UcCartItem��$ ItemName�6t   �� 54      ���#a49fb9$��11(�6l   $	��0 2q�6z   ��6   �� $ 
UcCartItem��$ ItemQuantity�6z   �� 55      ���#a49fb9$��13(�6i   ��6   �� $ 
UcCartItem��$ 	ItemPrice�6i   56      � 59      $��2��6    � RowDefinitions 5:      �� 5;      $��* �6$   �� 5<      $��* �6$   .+�5?      .  (MahApps.Metro.IconPacks.PackIconMaterial  $��0��6(      Kind$ PlusBox=�65   $��Center=�6E   $��20��6a   $��20��6l   ���#06c9956x     5@      $��1��6(   $ MinusBox=�65   $��Center=�6F   $��20��6b   $��20��6m   ���#06c9956y   5A      5B      5C      5D      