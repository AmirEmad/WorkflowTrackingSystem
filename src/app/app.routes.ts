import { Routes } from '@angular/router';
import { WorkflowListComponent } from './features/workflows/workflow-list/workflow-list.component';
import { ProcessListComponent } from './features/processes/process-list/process-list.component';
import { ProcessDetailComponent } from './features/processes/process-detail/process-detail.component';
import { WorkflowFormComponent } from './features/workflows/workflow-form/workflow-form.component';

export const routes: Routes = [
  { path: '', redirectTo: '/workflows', pathMatch: 'full' },
  { path: 'workflows', component: WorkflowListComponent },
  { path: 'workflows/create', component: WorkflowFormComponent },
  { path: 'workflows/:id', component: WorkflowFormComponent },
  { path: 'processes', component: ProcessListComponent },
  { path: 'processes/:id', component: ProcessDetailComponent },
  { path: '**', redirectTo: '/workflows' }
];