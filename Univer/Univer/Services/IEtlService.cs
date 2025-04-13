public interface IEtlService
{
    Task RunEtlProcess(int maxThreads = 5);
}