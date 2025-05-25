import { ProcessStatus } from "./process-status";
import { ProcessStep } from "./process-step";
import { Workflow } from "./workflow";

export interface Process {
    id: string;
  workflowId: string;
  initiator: string;
  status: ProcessStatus;
  currentStep?: string;
  createdAt: Date;
  updatedAt: Date;
  workflow: Workflow;
  processSteps: ProcessStep[];
}
