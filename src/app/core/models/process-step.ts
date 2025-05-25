import { StepStatus } from "./step-status";

export interface ProcessStep {
    id: string;
  processId: string;
  workflowStepId: string;
  stepName: string;
  performedBy?: string;
  action?: string;
  status: StepStatus;
  createdAt: Date;
  completedAt?: Date;
  validationResult?: string;
  comments?: string;
}
