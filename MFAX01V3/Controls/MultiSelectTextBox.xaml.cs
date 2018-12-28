using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Controls.Primitives;
using System.Collections;
using System.Threading;
using System.Text.RegularExpressions;

namespace MFAX01V3
{
    public class MyData : MultiSelectTextBox.IMultiSelctTextBoxData, IComparable
    {
        public string m_name = "";
        public string DisplayText
        {
            get { return m_name; }
            set
            {
                m_name = value;
            }
        }

        public MyData()
        {

        }

        public MyData(string name)
        {
            DisplayText = name;
        }

        public int CompareTo(object obj)
        {
            MyData val = obj as MyData;
            return String.Compare(m_name, val.m_name, true);
        }
    }
    public class EmailValidationRule : ValidationRule
    {
        public EmailValidationRule()
        {
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string Text = value as string;
            if (Text != null && Text.Length > 0 && !Regex.IsMatch((string)value, @"^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$"))
            {
                return new ValidationResult(false,
                  "Incorrect email format");
            }
            else
            {
                return new ValidationResult(true, null);
            }
        }
    }

    /// <summary>
    /// Interaction logic for MultiSelectTextBox.xaml
    /// </summary>
    public partial class MultiSelectTextBox : UserControl, INotifyPropertyChanged
    {
        public interface IMultiSelctTextBoxData
        {
            string DisplayText { get; set; }
        }

        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String info)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(info));
            }
        }
        #endregion

        #region Validation members

        public string Email
        {
            get;
            set;
        }


        private void validationError(object sender, ValidationErrorEventArgs e)
        {
            if (!m_popup.IsOpen && e.Action == ValidationErrorEventAction.Added)
            {
                MessageBox.Show(e.Error.ErrorContent.ToString());
            }
        }


        #endregion

        public static RoutedUICommand RemoveCommand = new RoutedUICommand("Remove", "RemoveCommand", typeof(MultiSelectTextBox));
        public static RoutedUICommand EditCommand = new RoutedUICommand("Edit", "EditCommand", typeof(MultiSelectTextBox));
        public static RoutedUICommand ManageCommand = new RoutedUICommand("Manage", "ManageCommand", typeof(MultiSelectTextBox));
        public static RoutedUICommand ShowDropDown = new RoutedUICommand("ShowDropDown", "ShowDropDownCommand", typeof(MultiSelectTextBox));

        public static readonly DependencyProperty AvailableListProperty = DependencyProperty.Register("AvailableList", typeof(ObservableCollection<MyData>), typeof(MultiSelectTextBox), new UIPropertyMetadata(AvailableList_Changed));

        public ObservableCollection<MyData> AvailableList
        {
            get { return (ObservableCollection<MyData>)GetValue(AvailableListProperty); }
            set { SetValue(AvailableListProperty, value); }
        }

        private static void AvailableList_Changed(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            MultiSelectTextBox thisControl = sender as MultiSelectTextBox;
            if (thisControl != null)
            {
                var theView = thisControl.GetListCollectionView();
                if (theView != null)
                {
                    theView.Filter = thisControl.SearchFilter;
                }
                thisControl.m_availableListLoaded = true;
                thisControl.m_triedToFillAvailableList = true;
            }
        }

        public static readonly DependencyProperty SelectedListProperty = DependencyProperty.Register("SelectedList", typeof(IList), typeof(MultiSelectTextBox), new UIPropertyMetadata(null));

        public IList SelectedList
        {
            get { return (IList)GetValue(SelectedListProperty); }
            set { SetValue(SelectedListProperty, value); }
        }

        public static readonly DependencyProperty AllowNewItemsProperty = DependencyProperty.Register("AllowNewItems", typeof(bool), typeof(MultiSelectTextBox), new UIPropertyMetadata(true));

        public bool AllowNewItems
        {
            get { return (bool)GetValue(AllowNewItemsProperty); }
            set { SetValue(AllowNewItemsProperty, value); }
        }

        public static readonly DependencyProperty MaxSelectedItemsProperty = DependencyProperty.Register("MaxSelectedItems", typeof(int?), typeof(MultiSelectTextBox), new UIPropertyMetadata(null));

        public int? MaxSelectedItems
        {
            get { return (int?)GetValue(MaxSelectedItemsProperty); }
            set { SetValue(MaxSelectedItemsProperty, value); }
        }

        public delegate object CreateObjectDelegate(string DisplayName);
        public event CreateObjectDelegate m_createObjectDelegate;

        bool m_triedToFillAvailableList = false;
        public delegate IList FillAvailableListDelegate();
        public event FillAvailableListDelegate m_fillAvailableListDelegate;

        public bool m_fetchingData = false;
        public bool FetchingData
        {
            get { return m_fetchingData; }
            set
            {
                m_fetchingData = value;
                NotifyPropertyChanged("FetchingData");
            }
        }

        public bool m_availableListLoaded = false;

        public void Refresh()
        {

            var theView = this.GetListCollectionView();
            if (theView != null)
            {
                theView.Refresh();
            }
        }

        public MultiSelectTextBox()
        {
            InitializeComponent();
            this.DataContext = this;

            m_popup.CustomPopupPlacementCallback += new CustomPopupPlacementCallback(PopupRepositioning);

            this.CommandBindings.Add(new CommandBinding(MultiSelectTextBox.RemoveCommand, RemoveCommand_Execute, RemoveCommand_CanExecute));
            this.CommandBindings.Add(new CommandBinding(MultiSelectTextBox.ShowDropDown, ShowDropDown_Execute, ShowDropDown_CanExecute));
        }

        private void RemoveCommand_Execute(object sender, ExecutedRoutedEventArgs e)
        {
            IMultiSelctTextBoxData d = e.Parameter as IMultiSelctTextBoxData;
            if (d != null)
            {
                SelectedList.Remove(d);

                Refresh();
            }
        }

        private void ShowDropDown_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void ShowDropDown_Execute(object sender, ExecutedRoutedEventArgs e)
        {
            m_popup.IsOpen = true;
        }

        private void RemoveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private CustomPopupPlacement[] PopupRepositioning(Size popupSize, Size targetSize, Point offset)
        {
            Point p = Mouse.GetPosition(this);
            var item = this.InputHitTest(p);
            if (item == null)
            {
                m_popup.IsOpen = false;
            }

            System.Diagnostics.Trace.WriteLine("PopupRepositioning - x: " + offset.X + " y: " + offset.Y + " height: " + root.ActualHeight + " - " + item);


            return new CustomPopupPlacement[] {
                new CustomPopupPlacement(new Point(0.01 - offset.X, root.ActualHeight - offset.Y), PopupPrimaryAxis.Horizontal) };
        }

        private bool SelectSelectedItem()
        {
            if (MaxSelectedItems.HasValue && SelectedList.Count >= MaxSelectedItems.Value)
            {
                return false;
            }

            if (m_availableListLoaded == false)
            {
                m_popup.IsOpen = true;
                return false;
            }

            string newName = string.Empty;
            bool handled = false;

            IMultiSelctTextBoxData newItem = null;

            if (m_popup.IsOpen)
            {
                var item = m_searchDataListBox.SelectedItem as IMultiSelctTextBoxData;
                if (item != null)
                {
                    newItem = item;
                }
            }
            else
            {
                newName = m_searchTextBox.Text;

                foreach (var item in AvailableList)
                {
                    IMultiSelctTextBoxData d = item as IMultiSelctTextBoxData;
                    if (d != null)
                    {
                        if (d.DisplayText.Equals(newName, StringComparison.InvariantCultureIgnoreCase))
                        {
                            newItem = d;
                            break;
                        }
                    }
                }
            }

            if (newItem == null &&
                AllowNewItems &&
                newName != string.Empty)
            {
                if (m_createObjectDelegate != null)
                {
                    var newObj = m_createObjectDelegate(newName);

                    if (newObj != null && newObj is IMultiSelctTextBoxData)
                    {
                        newItem = newObj as IMultiSelctTextBoxData;
                        Helpers.AddSorted(AvailableList, new MyData(newItem.DisplayText));
                    }
                }
            }

            if (newItem != null)
            {
                SelectedList.Add(newItem);

                m_searchTextBox.Text = string.Empty;
                m_searchTextBox.Focus();

                m_popup.IsOpen = false;

                handled = true;

                Refresh();
            }

            return handled;
        }

        private bool SearchFilter(object obj)
        {
            IMultiSelctTextBoxData d = obj as IMultiSelctTextBoxData;
            if (d != null)
            {
                if (SelectedList != null)
                {
                    foreach (var item in SelectedList)
                    {
                        IMultiSelctTextBoxData selected = item as IMultiSelctTextBoxData;
                        if (selected != null)
                        {
                            if (selected.DisplayText == d.DisplayText)
                            {
                                return false;
                            }
                        }
                    }
                }

                if (m_searchTextBox.Text == string.Empty)
                    return true;

                int index = d.DisplayText.IndexOf(m_searchTextBox.Text, 0, StringComparison.CurrentCultureIgnoreCase);
                if (index >= 0)
                {
                    return true;
                }
            }

            return false;
        }

        private ListCollectionView GetListCollectionView()
        {
            return (ListCollectionView)CollectionViewSource.GetDefaultView(this.AvailableList);
        }

        protected bool HasFocus()
        {
            DependencyObject focused = (this.IsKeyboardFocusWithin || m_popup.IsKeyboardFocusWithin) ?
                                                                        Keyboard.FocusedElement as DependencyObject :
                                                                        FocusManager.GetFocusedElement(this) as DependencyObject;

            while (focused != null)
            {
                if (object.ReferenceEquals(focused, this) ||
                    (object.ReferenceEquals(focused, m_searchDataListBox)))
                {
                    return true;
                }

                // This helps deal with popups that may not be in the same 
                // visual tree
                DependencyObject parent = VisualTreeHelper.GetParent(focused);
                if (parent == null)
                {
                    // Try the logical parent.
                    FrameworkElement element = focused as FrameworkElement;
                    if (element != null)
                    {
                        parent = element.Parent;
                    }
                }
                focused = parent;
            }
            return false;
        }


        private void m_searchDataListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            SelectSelectedItem();
        }

        private void m_searchTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            System.Diagnostics.Trace.WriteLine("m_searchTextBox_KeyDown" + e.Key + " Text: " + m_searchTextBox.Text);

            switch (e.Key)
            {
                case Key.Return:
                case Key.Tab:

                    if (!m_popup.IsOpen)
                    {
                        BindingExpression be = m_searchTextBox.GetBindingExpression(TextBox.TextProperty);
                        be.UpdateSource();
                        if (be.HasError)
                            return;
                    }

                    e.Handled = SelectSelectedItem();
                    break;
            }
        }

        private void m_searchTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            Refresh();

            System.Diagnostics.Trace.WriteLine("m_searchTextBox_KeyUp: " + e.Key + " Text: " + m_searchTextBox.Text);

            switch (e.Key)
            {
                case Key.Up:
                case Key.Down:
                case Key.Return:
                case Key.Tab:
                    break;
                default:
                    e.Handled = HandleKeys(e.Key, false);
                    break;
            }
        }

        private void m_searchTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            System.Diagnostics.Trace.WriteLine("m_searchTextBox_PreviewKeyDown: " + e.Key + " Text: " + m_searchTextBox.Text);

            switch (e.Key)
            {
                case Key.F5:
                    m_availableListLoaded = false;
                    m_triedToFillAvailableList = false;
                    AvailableList.Clear();
                    break;
                case Key.Back:
                case Key.Down:
                case Key.Up:
                    e.Handled = HandleKeys(e.Key, true);
                    break;
            }
        }

        private bool HandleKeys(System.Windows.Input.Key Key, bool previewDown)
        {
            bool handled = false;
            bool? showPopup = null;
            int? selectedIndex = null;

            switch (Key)
            {
                case Key.Down:
                    if (m_popup.IsOpen)
                    {
                        selectedIndex = m_searchDataListBox.SelectedIndex + 1;
                    }

                    showPopup = true;
                    handled = true;

                    break;
                case Key.Up:
                    if (m_popup.IsOpen)
                    {
                        selectedIndex = Math.Max(m_searchDataListBox.SelectedIndex - 1, 0);
                        showPopup = true;
                        handled = true;
                    }
                    break;

                case Key.F4:
                    if (m_popup.IsOpen)
                    {
                        showPopup = false;
                    }
                    else
                    {
                        showPopup = true;
                    }
                    handled = true;
                    break;
                case Key.Escape:
                    if (m_popup.IsOpen)
                    {
                        showPopup = false;
                        handled = true;
                    }
                    break;

                case Key.Back:
                    {
                        if (m_searchTextBox.Text == string.Empty)
                        {
                            if (previewDown)
                            {
                                if (SelectedList.Count > 0)
                                {
                                    IMultiSelctTextBoxData d = SelectedList[SelectedList.Count - 1] as IMultiSelctTextBoxData;
                                    SelectedList.RemoveAt(SelectedList.Count - 1);
                                    Refresh();

                                }
                            }
                            showPopup = false;
                            handled = true;
                        }
                    }
                    break;
            }

            if (showPopup.HasValue == false)
            {
                if (m_searchTextBox.Text == string.Empty)
                {
                    showPopup = false;
                }
                else
                {
                    var theView = this.GetListCollectionView();
                    if (theView == null || theView.Count > 0)
                    {
                        showPopup = true;
                    }
                    else
                    {
                        showPopup = false;
                    }
                }
            }

            if (showPopup.HasValue)
            {
                if (m_popup.IsOpen == false)
                {
                    selectedIndex = null;
                }

                m_popup.IsOpen = showPopup.Value;

                if (m_popup.IsOpen)
                {

                    if (selectedIndex.HasValue)
                    {
                        m_searchDataListBox.SelectedIndex = selectedIndex.Value;
                        m_searchDataListBox.ScrollIntoView(m_searchDataListBox.SelectedItem);
                    }
                    else
                    {
                        m_searchDataListBox.SelectedIndex = 0;
                    }
                }
            }

            return handled;
        }



        private void m_searchTextBox_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta > 0)
            {
                HandleKeys(Key.Up, false);
            }
            else if (e.Delta < 0)
            {
                HandleKeys(Key.Down, false);
            }
        }

        private void m_searchDataListBox_MouseUp(object sender, MouseButtonEventArgs e)
        {
            m_searchTextBox.Focus();
        }

    }
}
