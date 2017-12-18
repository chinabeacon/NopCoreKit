using System;

namespace ChinaBeacon.Sdk.Core.Data
{
    public interface IWorkFlowEngine
    {
        Guid WorkFlowBegin(string systenmWorkFlowDefineName, Guid customerId);
        bool WorkFlowRunNextStep(Guid instanceId, Guid customerId, string remarks);
        bool WorkFlowRunPreviousStep(Guid instanceId, Guid customerId, string remarks);

        Guid WorkFlowBeginUnDefine(Guid customerId, Guid nextAuditCustomerId);
        Guid WorkFlowBeginUnDefine(Guid instanceId, Guid customerId, Guid nextAuditCustomerId);

        bool WorkFlowRunNextStepUnDefine(bool isEnd, Guid instanceId, Guid customerId, Guid nextAuditCustomerId,
            string remarks);

        bool WorkFlowRunPreviousStepUnDefine(Guid instanceId, Guid customerId, string remarks);
    }
}
