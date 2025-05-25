import { Process } from "./process";
import { WorkflowStep } from "./workflow-step";

export interface Workflow {
  id: string;
  name: string;
  description: string;
  createdAt: Date;
  updatedAt: Date;
  steps: WorkflowStep[];
  processes?: Process[];
}
