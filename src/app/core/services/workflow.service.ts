import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Workflow } from '../models/workflow';
import { CreateWorkflowDto } from '../models/create-workflow-dto';

@Injectable({
  providedIn: 'root'
})
export class WorkflowService {
 // private readonly apiUrl = `${environment.apiUrl}/v1/workflows`;

  private readonly apiUrl = `https://localhost:44390/api/Workflows`;
  constructor(private http: HttpClient) { }
  getWorkflows(): Observable<Workflow[]> {
    return this.http.get<Workflow[]>(this.apiUrl);
  }

  getWorkflow(id: string): Observable<Workflow> {
    return this.http.get<Workflow>(`${this.apiUrl}/${id}`);
  }

  createWorkflow(workflow: CreateWorkflowDto): Observable<Workflow> {
    return this.http.post<Workflow>(this.apiUrl, workflow);
  }

  updateWorkflow(id: string, workflow: CreateWorkflowDto): Observable<Workflow> {
    return this.http.put<Workflow>(`${this.apiUrl}/${id}`, workflow);
  }

  deleteWorkflow(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
