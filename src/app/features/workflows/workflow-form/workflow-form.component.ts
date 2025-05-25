import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, FormArray, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { WorkflowService } from '../../../core/services/workflow.service';
import { NotificationService } from '../../../core/services/notification.service';
import { CreateWorkflowDto } from '../../../core/models/create-workflow-dto';

@Component({
  selector: 'app-workflow-form',  
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './workflow-form.component.html',
  styleUrl: './workflow-form.component.css'
})
export class WorkflowFormComponent implements OnInit{
 workflowForm!: FormGroup;
  isEditMode = false;
  isSubmitting = false;
  workflowId?: string;
  constructor(
    private fb: FormBuilder,
    private workflowService: WorkflowService,
    private notificationService: NotificationService,
    private router: Router,
    private route: ActivatedRoute
  ) {}

   ngOnInit() {
    this.initializeForm();
    this.checkEditMode();
  }
  initializeForm() {
    this.workflowForm = this.fb.group({
      name: ['', [Validators.required]],
      description: [''],
      steps: this.fb.array([this.createStepFormGroup()])
    });
  }

  createStepFormGroup(): FormGroup {
    return this.fb.group({
      stepName: ['', [Validators.required]],
      assignedTo: ['', [Validators.required]],
      actionType: ['', [Validators.required]],
      nextStep: [''],
      requiresValidation: [false]
    });
  }

  get steps(): FormArray {
    return this.workflowForm.get('steps') as FormArray;
  }

  addStep() {
    this.steps.push(this.createStepFormGroup());
  }

  removeStep(index: number) {
    if (this.steps.length > 1) {
      this.steps.removeAt(index);
    }
  }

  checkEditMode() {
    this.workflowId = this.route.snapshot.paramMap.get('id') || undefined;
    this.isEditMode = !!this.workflowId;

    if (this.isEditMode && this.workflowId) {
      this.loadWorkflow(this.workflowId);
    }
  }

  loadWorkflow(id: string) {
    this.workflowService.getWorkflow(id).subscribe({
      next: (workflow) => {
        this.workflowForm.patchValue({
          name: workflow.name,
          description: workflow.description
        });

        // Clear existing steps and add workflow steps
        while (this.steps.length !== 0) {
          this.steps.removeAt(0);
        }

        workflow.steps.forEach(step => {
          this.steps.push(this.fb.group({
            stepName: [step.stepName, [Validators.required]],
            assignedTo: [step.assignedTo, [Validators.required]],
            actionType: [step.actionType, [Validators.required]],
            nextStep: [step.nextStep || ''],
            requiresValidation: [step.requiresValidation]
          }));
        });
      },
      error: (error) => {
        this.notificationService.showError('Error', 'Failed to load workflow');
        this.goBack();
      }
    });
  }

  isFieldInvalid(fieldName: string): boolean {
    const field = this.workflowForm.get(fieldName);
    return !!(field && field.invalid && (field.dirty || field.touched));
  }

  isStepFieldInvalid(stepIndex: number, fieldName: string): boolean {
    const stepControl = this.steps.at(stepIndex);
    const field = stepControl.get(fieldName);
    return !!(field && field.invalid && (field.dirty || field.touched));
  }

  onSubmit() {
    if (this.workflowForm.valid && !this.isSubmitting) {
      this.isSubmitting = true;
      const workflowData: CreateWorkflowDto = this.workflowForm.value;

      const operation = this.isEditMode && this.workflowId
        ? this.workflowService.updateWorkflow(this.workflowId, workflowData)
        : this.workflowService.createWorkflow(workflowData);

      operation.subscribe({
        next: (workflow) => {
          const message = this.isEditMode ? 'Workflow updated successfully' : 'Workflow created successfully';
          this.notificationService.showSuccess('Success', message);
          this.router.navigate(['/workflows']);
        },
        error: (error) => {
          this.notificationService.showError('Error', 'Failed to save workflow');
          this.isSubmitting = false;
        }
      });
    }
  }

  goBack() {
    this.router.navigate(['/workflows']);
  }

}
