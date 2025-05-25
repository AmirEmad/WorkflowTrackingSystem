import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, ActivatedRoute, Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { Observable } from 'rxjs';
import { ProcessService } from '../../../core/services/process.service';
import { NotificationService } from '../../../core/services/notification.service';
import { Process } from '../../../core/models/process';
import { StepStatus } from '../../../core/models/step-status';
import { ExecuteStepDto } from '../../../core/models/execute-step-dto';

@Component({
  selector: 'app-process-detail',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule],
  templateUrl: './process-detail.component.html',
  styleUrl: './process-detail.component.css'
})
export class ProcessDetailComponent implements OnInit{
process$!: Observable<Process>;
  executeForm = {
    performedBy: '',
    action: '',
    comments: ''
  };

  readonly StepStatus = StepStatus;

  constructor(
    private processService: ProcessService,
    private notificationService: NotificationService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit() {
    const processId = this.route.snapshot.paramMap.get('id');
    if (processId) {
      this.process$ = this.processService.getProcess(processId);
    }
  }
  canExecuteStep(process: Process): boolean {
    return process.status === 1; // Active status
  }

  canSubmitExecution(): boolean {
    return !!(this.executeForm.performedBy && this.executeForm.action);
  }

  executeStep(process: Process) {
    if (this.canSubmitExecution() && process.currentStep) {
      const executeDto: ExecuteStepDto = {
        processId: process.id,
        stepName: process.currentStep,
        performedBy: this.executeForm.performedBy,
        action: this.executeForm.action,
        comments: this.executeForm.comments || undefined
      };

      this.processService.executeStep(executeDto).subscribe({
        next: (updatedProcess) => {
          this.notificationService.showSuccess('Success', 'Step executed successfully');
          this.executeForm = { performedBy: '', action: '', comments: '' };
          // Refresh the process data
          this.process$ = this.processService.getProcess(process.id);
        },
        error: (error) => {
          this.notificationService.showError('Error', 'Failed to execute step');
        }
      });
    }
  }

  goBack() {
    this.router.navigate(['/processes']);
  }

  getStatusText(status: number): string {
    switch (status) {
      case 1: return 'Active';
      case 2: return 'Completed';
      case 3: return 'Pending';
      case 4: return 'Rejected';
      default: return 'Unknown';
    }
  }

  getStatusClass(status: number): string {
    switch (status) {
      case 1: return 'active';
      case 2: return 'completed';
      case 3: return 'pending';
      case 4: return 'rejected';
      default: return 'active';
    }
  }

  getStepStatusText(status: StepStatus): string {
    switch (status) {
      case StepStatus.Pending: return 'Pending';
      case StepStatus.InProgress: return 'In Progress';
      case StepStatus.Completed: return 'Completed';
      case StepStatus.Rejected: return 'Rejected';
      case StepStatus.ValidationFailed: return 'Validation Failed';
      default: return 'Unknown';
    }
  }

  getStepStatusClass(status: StepStatus): string {
    switch (status) {
      case StepStatus.Pending: return 'pending';
      case StepStatus.InProgress: return 'active';
      case StepStatus.Completed: return 'completed';
      case StepStatus.Rejected: return 'rejected';
      case StepStatus.ValidationFailed: return 'rejected';
      default: return 'pending';
    }
  }
}
