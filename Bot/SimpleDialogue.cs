

namespace Bot
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.Bot.Builder.Dialogs;
    using Microsoft.Bot.Builder.Luis;

    [LuisModel("75e234b5-4413-4b8a-8532-4377f8121bee", "77a40b8333fe43ca8029a7b9343c6863")]
    [Serializable]
    public class SimpleDialog : LuisDialog
    {
        private readonly Dictionary<string, Alarm> alarmByWhat = new Dictionary<string, Alarm>();

        public const string DefaultAlarmWhat = "default";

        public string FindUser(LuisResult result)
        {
            string name;

            EntityRecommendation title;
            if (result.TryFindEntity(SimpleDialog.EntityContanctName, out title))
            {
                name = title.Entity;
            }
            else
            {
                name = DefaultAlarmWhat;
            }

            return name;
        }

        public const string EntityContanctName = "Company";
        public const string Entity_Alarm_Start_Time = "builtin.alarm.start_time";
        public const string Entity_Alarm_Start_Date = "builtin.alarm.start_date";

        [LuisIntent("None")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            string message = $"Sorry I did not understand: Ankit @ 917-558-4143 will help you";
            await context.PostAsync(message);
            context.Wait(MessageReceived);
        }

        [LuisIntent("CallCustomer")]        
        public async Task CallIntent(IDialogContext context, LuisResult result)
        {
            string name = this.FindUser(result);
            await context.PostAsync($"Calling {name} at 1-800-121123");
            context.Wait(MessageReceived);
        }
       

        [LuisIntent("GetInvoice")]
        public async Task GetInvoice(IDialogContext context, LuisResult result)
        {
            string name = this.FindUser(result);
            Random r = new Random();
            int amount = r.Next(1, 10000);
            await context.PostAsync($"Total Invoice for {name} as of now is ${amount}.00");
            context.Wait(MessageReceived);
        }

        [LuisIntent("builtin.intent.communication.make_call")]
        [LuisIntent("builtin.intent.places.get_phone_number")]
        public async Task CallIntent2(IDialogContext context, LuisResult result)
        {
            string name = this.FindUser(result);
            await context.PostAsync($"Calling {name} at 1-800-121123");
            context.Wait(MessageReceived);
        }

        public SimpleDialog(ILuisService service = null)
            : base(service)
        {
        }

        [Serializable]
        public sealed class Alarm : IEquatable<Alarm>
        {
            public DateTime When { get; set; }
            public string What { get; set; }

            public override string ToString()
            {
                return $"[{this.What} at {this.When}]";
            }

            public bool Equals(Alarm other)
            {
                return other != null
                    && this.When == other.When
                    && this.What == other.What;
            }

            public override bool Equals(object other)
            {
                return Equals(other as Alarm);
            }

            public override int GetHashCode()
            {
                return this.What.GetHashCode();
            }
        }
    }
}