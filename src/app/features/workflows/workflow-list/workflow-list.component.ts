import { Component, OnInit,Injectable, Injector } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { Observable } from 'rxjs';
import { Workflow } from '../../../core/models/workflow';
import { WorkflowService } from '../../../core/services/workflow.service';
import { NotificationService } from '../../../core/services/notification.service';


@Component({
  selector: 'app-workflow-list',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './workflow-list.component.html',
  styleUrl: './workflow-list.component.css'
})
export class WorkflowListComponent implements OnInit{
    workflows$!: Observable<Workflow[]>;

constructor(
    private workflowService: WorkflowService,
    private notificationService: NotificationService
  ) {}

  ngOnInit(): void {
    this.loadWorkflows();
  }
  loadWorkflows() {
    this.workflows$ = this.workflowService.getWorkflows();
  }

  deleteWorkflow(id: string) {
    if (confirm('Are you sure you want to delete this workflow?')) {
      this.workflowService.deleteWorkflow(id).subscribe({
        next: () => {
         this.notificationService.showSuccess('Success', 'Workflow deleted successfully');
          this.loadWorkflows();
        },
        error: (error) => {
          this.notificationService.showError('Error', 'Failed to delete workflow');
        }
      });
    }
  }

  startProcess(workflowId: string) {
    // Navigate to start process form or open modal
    // Implementation depends on UX requirements
  }
}
