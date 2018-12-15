using Model.DTO;
using Model.Utils;
using Newtonsoft.Json;

namespace Model.DataModel
{
    [JsonObject(MemberSerialization.OptIn)] //for serialisation into local store
    public class ReviewCollector
    {
        public ReviewCollector()
        {
            IsFinished = false;
        }

        [JsonProperty]
        public int UserId { get; set; }
        [JsonProperty]
        public int ConferenceId { get; set; }
        [JsonProperty]
        public ReviewStepOne StepOne { get; set; }
        [JsonProperty]
        public ReviewStepTwo StepTwo { get; set; }
        [JsonProperty]
        public ReviewStepThree StepThree{ get; set; }
        [JsonProperty]
        public ReviewStepFour StepFour { get; set; }
        [JsonProperty]
        public ReviewStepFive StepFive { get; set; }
        [JsonProperty]
        public ReviewFinal StepFinal{ get; set; }

        public int ReviewProgress
        {
            get
            {
                if (StepFinal != null)
                {
                    return 6;
                }
                if (StepFive != null)
                {
                    return 5;
                }
                if (StepFour != null)
                {
                    return 4;
                }
                if (StepThree != null)
                {
                    return 3;
                }
                if (StepTwo != null)
                {
                    return 2;
                }
                if (StepOne != null)
                {
                    return 1;
                }
                return 0;
            }
        }

        public bool IsFinished { get; set; }

        public static explicit operator QuestionnaireModel(ReviewCollector x)
        {
            return ModelConverter.CastToDTO(x);
        }
    }
}
