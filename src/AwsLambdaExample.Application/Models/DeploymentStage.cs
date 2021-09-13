using System.Collections.Generic;

namespace AwsLambdaExample.Application.Models
{
    public class DeploymentStage
    {
        private static readonly IDictionary<string, DeploymentStage> _stages = new Dictionary<string, DeploymentStage> {
            { "prd", new DeploymentStage("prd")},
            { "stg", new DeploymentStage("stg")},
            { "uat", new DeploymentStage("uat")},
            { "sit", new DeploymentStage("sit")},
            { "dev", new DeploymentStage("dev")},
            { "local", new DeploymentStage("local")},
        };

        private DeploymentStage(string value)
        {
            Value = value;
        }

        public static DeploymentStage Prd = _stages["prd"];

        public static DeploymentStage Stg = _stages["stg"];

        public static DeploymentStage Uat = _stages["uat"];

        public static DeploymentStage Sit = _stages["sit"];

        public static DeploymentStage Dev = _stages["dev"];

        public static DeploymentStage Local = _stages["local"];

        public static DeploymentStage FromValue(string value)
        {
            return _stages[value];
        }

        public string Value { get; }

        public override string ToString()
        {
            return Value;
        }

        public override bool Equals(object? obj)
        {
            //Check for null and compare run-time types.
            if ((obj == null) || !GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                return Equals((DeploymentStage)obj);
            }
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }
}
