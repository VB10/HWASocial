using System;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Views;

namespace BoshokuDemo1.Droid.Dependency
{
    public class BadgeDrawable : Drawable
    {

        private const string BadgeValueOverflow = "*";
        private Paint badgeBackground;
        private Paint badgeText;
        private Rect _textRect = new Rect();
        private string _badgeValue = "";
        private bool _shouldDraw = true;
        private Context context;

        public override int Opacity => (int)Format.Unknown;

        public BadgeDrawable(Context context , Color backgroundColor,Color textColor){
            this.context = context;
            float textSize = 30;
            this.badgeBackground = new Paint();
            this.badgeBackground.Color = backgroundColor;
            this.badgeBackground.AntiAlias = true;
            this.badgeBackground.SetStyle(Paint.Style.Fill);

            badgeText = new Paint();
            badgeText.Color = textColor;
            badgeText.SetTypeface(Typeface.Default);
            badgeText.TextSize = textSize;
            badgeText.AntiAlias = true;
            badgeText.TextAlign = Paint.Align.Center;

        }
        public override void SetAlpha(int alpha)
        {
            // do nothing
        }

        public override void SetColorFilter(ColorFilter cf)
        {
            // do nothing
        }

        public override void Draw(Canvas canvas)
        {
            if (!_shouldDraw)
            {
                return;
            }
            Rect bounds = Bounds;
            float width = bounds.Right - bounds.Left;
            float height = bounds.Bottom - bounds.Top;
            float oneDp = 1 * context.Resources.DisplayMetrics.Density;

            // Position the badge in the top-right quadrant of the icon.
            float radius = ((Java.Lang.Math.Max(width, height) / 2)) / 2;
            float centerX = (width - radius - 1) + oneDp * 2;
            float centerY = radius - 2 * oneDp;
            canvas.DrawCircle(centerX, centerY, (int)(radius + oneDp * 5), badgeBackground);

            // Draw badge count message inside the circle.
            badgeText.GetTextBounds(_badgeValue, 0, _badgeValue.Length, _textRect);
            float textHeight = _textRect.Bottom - _textRect.Top;
            float textY = centerY + (textHeight / 2f);
            canvas.DrawText(_badgeValue.Length > 2 ? BadgeValueOverflow : _badgeValue,
                    centerX, textY, badgeText);
        }
        private void SetBadgeText(string text)
        {
            _badgeValue = text;

            // Only draw a badge if the value isn't a zero
            _shouldDraw = !text.Equals("0");
            InvalidateSelf();
        }
        public static void SetBadgeCount(Context context, IMenuItem item, int count, Color backgroundColor, Color textColor)
        {
            SetBadgeText(context, item, $"{count}", backgroundColor, textColor);
        }
        public static void SetBadgeText(Context context, IMenuItem item, string text, Color backgroundColor, Color textColor)
        {


            if (item.Icon == null)
            {
                return;
            }

            BadgeDrawable badge = null;
            Drawable icon = item.Icon;


            if (item.Icon is LayerDrawable)
            {

                LayerDrawable lDrawable = item.Icon as LayerDrawable;

                if (string.IsNullOrEmpty(text) || text == "0")
                {
                    icon = lDrawable.GetDrawable(0);
                    lDrawable.Dispose();
                }
                else
                {
                    for (var i = 0; i < lDrawable.NumberOfLayers; i++)
                    {
                        if (lDrawable.GetDrawable(i) is BadgeDrawable)
                        {
                            badge = lDrawable.GetDrawable(i) as BadgeDrawable;
                            break;
                        }

                    }

                    if (badge == null)
                    {
                        badge = new BadgeDrawable(context, backgroundColor, textColor);
                        icon = new LayerDrawable(new Drawable[] { item.Icon, badge });
                    }
                }

            }
            else
            {
                badge = new BadgeDrawable(context, backgroundColor, textColor);
                icon = new LayerDrawable(new Drawable[] { item.Icon, badge });
            }

            badge?.SetBadgeText(text);

            item.SetIcon(icon);
            icon.Dispose();
        }
    }
}
