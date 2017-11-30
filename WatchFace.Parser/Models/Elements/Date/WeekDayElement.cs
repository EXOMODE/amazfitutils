using System;
using System.Collections.Generic;
using System.Drawing;
using WatchFace.Parser.Interfaces;

namespace WatchFace.Parser.Models.Elements
{
    public class WeekDayElement : ImageSetElement
    {
        private static readonly Dictionary<DayOfWeek, int> DaysOfWeek = new Dictionary<DayOfWeek, int>
        {
            {DayOfWeek.Monday, 0},
            {DayOfWeek.Tuesday, 1},
            {DayOfWeek.Wednesday, 2},
            {DayOfWeek.Thursday, 3},
            {DayOfWeek.Friday, 4},
            {DayOfWeek.Saturday, 5},
            {DayOfWeek.Sunday, 6}
        };

        public WeekDayElement(Parameter parameter, Element parent, string name = null) :
            base(parameter, parent, name) { }

        public override void Draw(Graphics drawer, Bitmap[] resources, WatchState state)
        {
            Draw(drawer, resources, DaysOfWeek[state.Time.DayOfWeek]);
        }
    }
}