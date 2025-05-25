export interface ExecuteStepDto {
    processId: string;
  stepName: string;
  performedBy: string;
  action: string;
  comments?: string;
}
