using Xamarin.Forms;

namespace SafeTodoExample.Controls
{
    public class CardView : Frame
    {
        public CardView()
        {
            if (Device.RuntimePlatform == Device.iOS)
            {
                HasShadow = false;
            }
        }
    }
}
