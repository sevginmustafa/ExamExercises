using RobotService.Models.Robots.Contracts;
using System;

namespace RobotService.Models.Procedures
{
    public class Chip : Procedure
    {
        public override void DoService(IRobot robot, int procedureTime)
        {
            base.DoService(robot, procedureTime);

            if (robot.IsChipped)
            {
                throw new ArgumentException($"{robot.Name} is already chipped");
            }

            robot.IsChipped = true;
            robot.Happiness -= 5;
        }
    }
}
