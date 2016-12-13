using Windows.UI.Xaml.Controls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace AVL.Controls
{
    public sealed partial class CreateOrEditCategoryControl : UserControl
    {
        public CreateOrEditCategoryControl()
        {
            this.InitializeComponent();
        }

        public void SetContent(CreateOrEditCategoryViewModel model)
        {
            this.DataContext = model;
        }
    }
}
