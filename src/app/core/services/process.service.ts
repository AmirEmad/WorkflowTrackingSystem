import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Process } from '../models/process';
import { ProcessStatus } from '../models/process-status';
import { StartProcessDto } from '../models/start-process-dto';
import { ExecuteStepDto } from '../models/execute-step-dto';

@Injectable({
  providedIn: 'root'
})
export class ProcessService {
    private readonly apiUrl2 = `https://localhost:44390/api/Processes`;

  constructor(private http: HttpClient) { }
  
  getProcesses(workflowId?: string, status?: ProcessStatus, assignedTo?: string): Observable<Process[]> {
    let params = new HttpParams();
    
    if (workflowId) params = params.set('workflowId', workflowId);
    if (status !== undefined) params = params.set('status', status.toString());
    if (assignedTo) params = params.set('assignedTo', assignedTo);

    return this.http.get<Process[]>(this.apiUrl2, { params });
  }

  getProcess(id: string): Observable<Process> {
    return this.http.get<Process>(`${this.apiUrl2}/${id}`);
  }

  startProcess(dto: StartProcessDto): Observable<Process> {
    return this.http.post<Process>(`${this.apiUrl2}/start`, dto);
  }

  executeStep(dto: ExecuteStepDto): Observable<Process> {
    return this.http.post<Process>(`${this.apiUrl2}/execute`, dto);
  }
}
