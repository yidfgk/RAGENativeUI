using System;
using System.Drawing;

namespace RAGENativeUI.Elements
{                
    /// <summary>
    /// Simple item with a label.
    /// </summary>
    public class UIMenuItem
    {
        public static readonly Color DefaultBackColor = Color.Empty,
                                     DefaultHighlightedBackColor = Color.White,
                                     DefaultForeColor = Color.WhiteSmoke,
                                     DefaultHighlightedForeColor = Color.Black;

        [Obsolete] protected ResRectangle _rectangle;
        [Obsolete] protected ResText _text;
        [Obsolete] protected Sprite _selectedSprite;

        protected Sprite _badgeLeft;
        protected Sprite _badgeRight;

        [Obsolete] protected ResText _labelText;

        /// <summary>
        /// Called when user selects the current item.
        /// </summary>
        public event ItemActivatedEvent Activated;


        public Color BackColor { get; set; } = DefaultBackColor;
        public Color HighlightedBackColor { get; set; } = DefaultHighlightedBackColor;

        public Color ForeColor { get; set; } = DefaultForeColor;
        public Color HighlightedForeColor { get; set; } = DefaultHighlightedForeColor;

        /// <summary>
        /// Basic menu button.
        /// </summary>
        /// <param name="text">Button label.</param>
        public UIMenuItem(string text) : this(text, "")
        {
        }

        /// <summary>
        /// Basic menu button.
        /// </summary>
        /// <param name="text">Button label.</param>
        /// <param name="description">Description.</param>
        public UIMenuItem(string text, string description)
        {
            Enabled = true;

            _rectangle = new ResRectangle(new Point(0, 0), new Size(431, 38), Color.FromArgb(150, 0, 0, 0));
            _text = new ResText(text, new Point(8, 0), 0.33f, Color.WhiteSmoke, Common.EFont.ChaletLondon, ResText.Alignment.Left);
            Description = description;
            _selectedSprite = new Sprite("commonmenu", "gradient_nav", new Point(0, 0), new Size(431, 38));

            _badgeLeft = new Sprite("commonmenu", "", new Point(0, 0), new Size(40, 40));
            _badgeRight = new Sprite("commonmenu", "", new Point(0, 0), new Size(40, 40));

            _labelText = new ResText("", new Point(0, 0), 0.35f) {TextAlignment = ResText.Alignment.Right};
        }


        /// <summary>
        /// Whether this item is currently selected.
        /// </summary>
        public virtual bool Selected { get; set; }


        /// <summary>
        /// Whether this item is currently being hovered on with a mouse.
        /// </summary>
        public virtual bool Hovered { get; set; }


        /// <summary>
        /// This item's description.
        /// </summary>
        public virtual string Description { get; set; }


        /// <summary>
        /// Whether this item is enabled or disabled (text is greyed out and you cannot select it).
        /// </summary>
        public virtual bool Enabled { get; set; }

        internal virtual void ItemActivate(UIMenu sender)
        {
            Activated?.Invoke(sender, this);
        }
        
        /// <summary>
        /// Set item's position.
        /// </summary>
        /// <param name="y"></param>
        [Obsolete("Use UIMenuItem.SetVerticalPosition instead.")]
        public virtual void Position(int y)
        {
            SetVerticalPosition(y);
        }

        /// <summary>
        /// Set item's vertical position.
        /// </summary>
        /// <param name="y"></param>
        [Obsolete]
        public virtual void SetVerticalPosition(int y)
        {
            _rectangle.Position = new Point(Offset.X, y + 144 + Offset.Y);
            _selectedSprite.Position = new Point(0 + Offset.X, y + 144 + Offset.Y);
            _text.Position = new Point(8 + Offset.X, y + 147 + Offset.Y);

            _badgeLeft.Position = new Point(0 + Offset.X, y + 142 + Offset.Y);
            _badgeRight.Position = new Point(385 + Offset.X, y + 142 + Offset.Y);

            _labelText.Position = new Point(420 + Offset.X, y + 148 + Offset.Y);
        }

        /// <summary>
        /// Draw this item.
        /// </summary>
        [Obsolete]
        public virtual void Draw()
        {
            _rectangle.Size = new Size(431 + Parent.WidthOffset, 38);
            _selectedSprite.Size = new Size(431 + Parent.WidthOffset, 38);

            if (Hovered && !Selected)
            {
                _rectangle.Color = Color.FromArgb(20, 255, 255, 255);
                _rectangle.Draw();
            }

            _selectedSprite.Color = Selected ? HighlightedBackColor : BackColor;
            _selectedSprite.Draw();

            _text.Color = Enabled ? Selected ? HighlightedForeColor : ForeColor : Color.FromArgb(163, 159, 148);

            if (LeftBadge != BadgeStyle.None)
            {
                _text.Position = new Point(35 + Offset.X, _text.Position.Y);
                _badgeLeft.TextureDictionary = BadgeToSpriteLib(LeftBadge);
                _badgeLeft.TextureName = BadgeToSpriteName(LeftBadge, Selected);
                _badgeLeft.Color = IsBagdeWhiteSprite(LeftBadge) ? Enabled ? Selected ? HighlightedForeColor : ForeColor : Color.FromArgb(163, 159, 148) : Color.White;
                _badgeLeft.Draw();
            }
            else
            {
                _text.Position = new Point(8 + Offset.X, _text.Position.Y);
            }

            if (RightBadge != BadgeStyle.None)
            {
                _badgeRight.Position = new Point(385 + Offset.X + Parent.WidthOffset, _badgeRight.Position.Y);
                _badgeRight.TextureDictionary = BadgeToSpriteLib(RightBadge);
                _badgeRight.TextureName = BadgeToSpriteName(RightBadge, Selected);
                _badgeRight.Color = IsBagdeWhiteSprite(RightBadge) ? Enabled ? Selected ? HighlightedForeColor : ForeColor : Color.FromArgb(163, 159, 148) : Color.White;
                _badgeRight.Draw();
            }

            if (!String.IsNullOrWhiteSpace(RightLabel))
            {
                _labelText.Position = new Point(420 + Offset.X + Parent.WidthOffset, _labelText.Position.Y);
                _labelText.Caption = RightLabel;
                _labelText.Color = _text.Color = Enabled ? Selected ? HighlightedForeColor : ForeColor : Color.FromArgb(163, 159, 148);
                _labelText.Draw();
            }
            _text.Draw();
        }

        public virtual void Draw(float x, float y, float menuWidth, float itemHeight)
        {
            float rectWidth = menuWidth;
            float rectHeight = itemHeight;
            float rectX = x + rectWidth * 0.5f;
            float rectY = y + rectHeight * 0.5f;

            Color barColor = Selected ? HighlightedBackColor : BackColor;
            if (barColor != Color.Empty)
            {
                Parent.DrawSprite(_selectedSprite.TextureDictionary, _selectedSprite.TextureName,
                                  rectX, rectY,
                                  rectWidth, rectHeight,
                                  barColor);
            }

            if (Hovered && !Selected)
            {
                Color hoveredColor = Color.FromArgb(20, 255, 255, 255);
                Parent.DrawRect(rectX, rectY,
                                rectWidth, rectHeight,
                                hoveredColor);
            }

            Color textColor = GetItemTextColor();
            N.SetTextColour(textColor.R, textColor.G, textColor.B, textColor.A);
            N.SetTextScale(0f, 0.35f);
            N.SetTextJustification(1);
            N.SetTextFont(0);
            N.SetTextWrap(0f, 1f);
            N.SetTextCentre(false);
            N.SetTextDropshadow(0, 0, 0, 0, 0);
            N.SetTextEdge(0, 0, 0, 0, 0);

            N.BeginTextCommandDisplayText("STRING");
            N.AddTextComponentSubstringPlayerName(Text);
            N.EndTextCommandDisplayText(x + 0.0046875f, y + 0.00277776f);

            // TODO: LeftBadge
            // TODO: RightBadge
            // TODO: RightLabel
        }

        protected Color GetItemTextColor()
        {
            if (Enabled)
            {
                if (Selected)
                {
                    return HighlightedForeColor;
                }
                else
                {
                    return ForeColor;
                }
            }
            else
            {
                return Color.FromArgb(163, 159, 148);
            }
        }


        /// <summary>
        /// This item's offset.
        /// </summary>
        public Point Offset { get; set; }


        /// <summary>
        /// Returns this item's label.
        /// </summary>
        public string Text
        {
            get { return _text.Caption; }
            set { _text.Caption = value; }
        }


        /// <summary>
        /// Set the left badge. Set it to None to remove the badge.
        /// </summary>
        /// <param name="badge"></param>
        public virtual void SetLeftBadge(BadgeStyle badge)
        {
            LeftBadge = badge;
        }


        /// <summary>
        /// Set the right badge. Set it to None to remove the badge.
        /// </summary>
        /// <param name="badge"></param>
        public virtual void SetRightBadge(BadgeStyle badge)
        {
            RightBadge = badge;
        }


        /// <summary>
        /// Set the right label.
        /// </summary>
        /// <param name="text">Text as label. Set it to "" to remove the label.</param>
        public virtual void SetRightLabel(string text)
        {
            RightLabel = text;
        }

        /// <summary>
        /// Returns the current right label.
        /// </summary>
        public virtual string RightLabel { get; private set; }


        /// <summary>
        /// Returns the current left badge.
        /// </summary>
        public virtual BadgeStyle LeftBadge { get; private set; }


        /// <summary>
        /// Returns the current right badge.
        /// </summary>
        public virtual BadgeStyle RightBadge { get; private set; }

        public enum BadgeStyle
        {
            None,
            BronzeMedal,
            GoldMedal,
            SilverMedal,
            Alert,
            Crown,
            Ammo,
            Armour,
            Barber,
            Clothes,
            Franklin,
            Bike,
            Car,
            Gun,
            Heart,
            Makeup,
            Mask,
            Michael,
            Star,
            Tatoo,
            Trevor,
            Lock,
            Tick,
        }

        internal static string BadgeToSpriteLib(BadgeStyle badge)
        {
            switch (badge)
            {
                default:
                    return "commonmenu";
            }   
        }

        internal static string BadgeToSpriteName(BadgeStyle badge, bool selected)
        {
            switch (badge)
            {
                case BadgeStyle.None:
                    return "";
                case BadgeStyle.BronzeMedal:
                    return "mp_medal_bronze";
                case BadgeStyle.GoldMedal:
                    return "mp_medal_gold";
                case BadgeStyle.SilverMedal:
                    return "medal_silver";
                case BadgeStyle.Alert:
                    return "mp_alerttriangle";
                case BadgeStyle.Crown:
                    return "mp_hostcrown";
                case BadgeStyle.Ammo:
                    return selected ? "shop_ammo_icon_b" : "shop_ammo_icon_a";
                case BadgeStyle.Armour:
                    return selected ? "shop_armour_icon_b" : "shop_armour_icon_a";
                case BadgeStyle.Barber:
                    return selected ? "shop_barber_icon_b" : "shop_barber_icon_a";
                case BadgeStyle.Clothes:
                    return selected ? "shop_clothing_icon_b" : "shop_clothing_icon_a";
                case BadgeStyle.Franklin:
                    return selected ? "shop_franklin_icon_b" : "shop_franklin_icon_a";
                case BadgeStyle.Bike:
                    return selected ? "shop_garage_bike_icon_b" : "shop_garage_bike_icon_a";
                case BadgeStyle.Car:
                    return selected ? "shop_garage_icon_b" : "shop_garage_icon_a";
                case BadgeStyle.Gun:
                    return selected ? "shop_gunclub_icon_b" : "shop_gunclub_icon_a";
                case BadgeStyle.Heart:
                    return selected ? "shop_health_icon_b" : "shop_health_icon_a";
                case BadgeStyle.Lock:
                    return "shop_lock";
                case BadgeStyle.Makeup:
                    return selected ? "shop_makeup_icon_b" : "shop_makeup_icon_a";
                case BadgeStyle.Mask:
                    return selected ? "shop_mask_icon_b" : "shop_mask_icon_a";
                case BadgeStyle.Michael:
                    return selected ? "shop_michael_icon_b" : "shop_michael_icon_a";
                case BadgeStyle.Star:
                    return "shop_new_star";
                case BadgeStyle.Tatoo:
                    return selected ? "shop_tattoos_icon_b" : "shop_tattoos_icon_";
                case BadgeStyle.Tick:
                    return "shop_tick_icon";
                case BadgeStyle.Trevor:
                    return selected ? "shop_trevor_icon_b" : "shop_trevor_icon_a";
                default:
                    return "";
            }
        }

        internal static bool IsBagdeWhiteSprite(BadgeStyle badge/*, bool selected*/)
        {
            switch (badge)
            {
                case BadgeStyle.Lock:
                case BadgeStyle.Tick:
                case BadgeStyle.Crown:
                    return true;
                default:
                    return false;
            }
        }

        internal static Color BadgeToColor(BadgeStyle badge, bool selected)
        {
            switch (badge)
            {
                case BadgeStyle.Lock:
                case BadgeStyle.Tick:
                case BadgeStyle.Crown:
                    return selected ? Color.FromArgb(255, 0, 0, 0) : Color.FromArgb(255, 255, 255, 255);
                default:
                    return Color.FromArgb(255, 255, 255, 255);
            }
        }

        /// <summary>
        /// Returns the menu this item is in.
        /// </summary>
        public UIMenu Parent { get; set; }
    }
}

