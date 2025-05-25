import { CreateWorkflowStepDto } from "./create-workflow-step-dto";

export interface CreateWorkflowDto {
    name: string;
  description: string;
  steps: CreateWorkflowStepDto[];
}
