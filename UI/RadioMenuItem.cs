using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace tarkov_novi.UI
{
    public class RadioMenuItem : MenuItem
    {
        private bool abortCheckChange = false;

        public string GroupName
        {
            get => (string)GetValue(GroupNameProperty);
            set => SetValue(GroupNameProperty, value);
        }

        public static readonly DependencyProperty GroupNameProperty =
            DependencyProperty.Register(nameof(GroupName), typeof(string), typeof(RadioMenuItem),
                new PropertyMetadata("", (d, e) => ((RadioMenuItem)d).OnGroupNameChanged((string)e.OldValue, (string)e.NewValue)));

        static RadioMenuItem()
        {
            IsCheckedProperty.OverrideMetadata(typeof(RadioMenuItem),
                new FrameworkPropertyMetadata(null, (d, o) => ((RadioMenuItem)d).abortCheckChange ? d.GetValue(IsCheckedProperty) : o));
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new RadioMenuItem();
        }

        protected override void OnClick()
        {
            //This will handle correctly the click, but prevents the unchecking.
            //So the menu item acts that is correctly clicked (e.g. the menu disappears
            //but the user can only check, not uncheck the item.
            if (IsCheckable && IsChecked) abortCheckChange = true;
            base.OnClick();
            abortCheckChange = false;
        }

        protected override void OnChecked(RoutedEventArgs e)
        {
            base.OnChecked(e);
            //If the menu item is checked, other items of the same group will be unchecked.
            if (IsChecked) UncheckOtherGroupItems();
        }

        protected virtual void OnGroupNameChanged(string oldGroupName, string newGroupName)
        {
            //If the menu item enters on another group and is checked, other items will be unchecked.
            if (IsChecked) UncheckOtherGroupItems();
        }
        private void UncheckOtherGroupItems()
        {
            if (IsCheckable)
            {
                IEnumerable<RadioMenuItem> radioItems = Parent is ItemsControl parent ? parent.Items.OfType<RadioMenuItem>()
                    .Where((item) => item.IsCheckable && (item.DataContext == parent.DataContext || item.DataContext != DataContext)) : null;

                if (radioItems != null)
                {
                    foreach (RadioMenuItem item in radioItems)
                    {
                        if (item != this && item.GroupName == GroupName)
                        {
                            //This will uncheck all other items on the same group.
                            item.IsChecked = false;
                        }
                    }
                }
            }
        }
    }
}
