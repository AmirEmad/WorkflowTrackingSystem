import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { Observable, BehaviorSubject, combineLatest } from 'rxjs';
import { map, startWith } from 'rxjs/operators';
import { ProcessService } from '../../../core/services/process.service';
import { WorkflowService } from '../../../core/services/workflow.service';
import { NotificationService } from '../../../core/services/notification.service';
import { Process } from '../../../core/models/process';
import { Workflow } from '../../../core/models/workflow';
import { ProcessStatus } from '../../../core/models/process-status';

@Component({
  selector: 'app-process-list',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule],
  templateUrl: './process-list.component.html',
  styleUrl: './process-list.component.css'
})
export class ProcessListComponent implements OnInit{

  processes$!: Observable<Process[]>;
  workflows$!: Observable<Workflow[]>;
  filteredProcesses$!: Observable<Process[]>;
  
  private filtersSubject = new BehaviorSubject<{
    workflowId?: string;
    status?: ProcessStatus;
    assignedTo?: string;
  }>({});

  selectedWorkflowId: string = '';
  selectedStatus: ProcessStatus | '' = '';
  assignedTo: string = '';

  showStartProcessModal = false;
  newProcessWorkflowId: string = '';
  newProcessInitiator: string = '';

  readonly ProcessStatus = ProcessStatus;

  constructor(
    private processService: ProcessService,
    private workflowService: WorkflowService,
    private notificationService: NotificationService
  ) {}

  ngOnInit() {
    this.loadData();
    this.setupFilteredProcesses();
  }
loadData() {
    this.workflows$ = this.workflowService.getWorkflows();
    this.processes$ = this.processService.getProcesses();
  }

  setupFilteredProcesses() {
    this.filteredProcesses$ = combineLatest([
      this.processes$,
      this.filtersSubject.asObservable()
    ]).pipe(
      map(([processes, filters]) => {
        return processes.filter(process => {
          if (filters.workflowId && process.workflowId !== filters.workflowId) {
            return false;
          }
          if (filters.status !== undefined && process.status !== filters.status) {
            return false;
          }
          if (filters.assignedTo) {
            // This would need to check current step assignment in a real implementation
            return true;
          }
          return true;
        });
      })
    );
  }

  applyFilters() {
    this.filtersSubject.next({
      workflowId: this.selectedWorkflowId || undefined,
      status: this.selectedStatus || undefined,
      assignedTo: this.assignedTo || undefined
    });
  }

  startProcess() {
    if (this.newProcessWorkflowId && this.newProcessInitiator) {
      this.processService.startProcess({
        workflowId: this.newProcessWorkflowId,
        initiator: this.newProcessInitiator
      }).subscribe({
        next: (process) => {
          this.notificationService.showSuccess('Success', 'Process started successfully');
          this.closeModal();
          this.loadData();
        },
        error: (error) => {
          this.notificationService.showError('Error', 'Failed to start process');
        }
      });
    }
  }

  executeStep(process: Process) {
    // This would open a modal or navigate to step execution page
    // For now, we'll just show a notification
    this.notificationService.showInfo('Info', 'Step execution functionality would be implemented here');
  }

  closeModal() {
    this.showStartProcessModal = false;
    this.newProcessWorkflowId = '';
    this.newProcessInitiator = '';
  }

  getStatusText(status: ProcessStatus): string {
    switch (status) {
      case ProcessStatus.Active: return 'Active';
      case ProcessStatus.Completed: return 'Completed';
      case ProcessStatus.Pending: return 'Pending';
      case ProcessStatus.Rejected: return 'Rejected';
      default: return 'Unknown';
    }
  }

  getStatusClass(status: ProcessStatus): string {
    switch (status) {
      case ProcessStatus.Active: return 'active';
      case ProcessStatus.Completed: return 'completed';
      case ProcessStatus.Pending: return 'pending';
      case ProcessStatus.Rejected: return 'rejected';
      default: return 'active';
    }
  }
}

