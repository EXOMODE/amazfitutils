using System.Drawing;
using WatchFace.Parser.Interfaces;

namespace WatchFace.Parser.Models.Elements
{
    public class SeparateMonthAndDayElement : CompositeElement, IDrawable
    {
        public SeparateMonthAndDayElement(Parameter parameter, Element parent = null, string name = null) :
            base(parameter, parent, name) { }

        public NumberElement Month { get; set; }
        public NumberElement Day { get; set; }

        public void Draw(Graphics drawer, Bitmap[] resources, WatchState state)
        {
            var monthAndDay = (MonthAndDayElement) _parent;
            Month?.Draw(drawer, resources, state.Time.Month, monthAndDay.TwoDigitsMonth ? 2 : 1);
            Day?.Draw(drawer, resources, state.Time.Day, monthAndDay.TwoDigitsDay ? 2 : 1);
        }

        protected override Element CreateChildForParameter(Parameter parameter)
        {
            switch (parameter.Id)
            {
                case 1:
                    Month = new NumberElement(parameter, this);
                    return Month;
                case 3:
                    Day = new NumberElement(parameter, this);
                    return Day;
                default:
                    return base.CreateChildForParameter(parameter);
            }
        }
    }
}