namespace SoftwareEngineeringLab.Core.Solid.InterfaceSegregation;

// ==================== INTERFACE SEGREGATION PRINCIPLE (ISP) ====================
// "Clients should not be forced to depend on interfaces they do not use."

public static class InterfaceSegregationStudy
{
    // ❌ VIOLATION: Fat interface forces implementors to implement methods they don't support.
    public interface IWorkerFat
    {
        void Work();
        void Eat();
        void Sleep();
    }

    public class RobotWorkerBad : IWorkerFat
    {
        public void Work() { /* executes work */ }
        public void Eat() => throw new NotImplementedException("Robots do not eat.");
        public void Sleep() => throw new NotImplementedException("Robots do not sleep.");
    }

    //  REFACTORED: Segregated, role-focused interfaces.

    public interface IWorkable
    {
        void Work();
    }

    public interface IEatable
    {
        void Eat();
    }

    public interface ISleepable
    {
        void Sleep();
    }

    public class HumanWorker : IWorkable, IEatable, ISleepable
    {
        public void Work() { }
        public void Eat() { }
        public void Sleep() { }
    }

    public class RobotWorker : IWorkable
    {
        public void Work() { }
        // Clean: Not forced to stub Eat or Sleep!
    }
}
