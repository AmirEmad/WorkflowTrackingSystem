export interface CreateWorkflowStepDto {
    stepName: string;
  assignedTo: string;
  actionType: string;
  nextStep?: string;
  requiresValidation: boolean;
}
