export interface WorkflowStep {
    id: string;
  workflowId: string;
  stepName: string;
  assignedTo: string;
  actionType: string;
  nextStep?: string;
  order: number;
  requiresValidation: boolean;
}
