import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { Contact, ContactRequest } from '../models/contact.model';
import { ContactListQuery } from '../models/contact-list-query.model';
import { PagedResult } from '../models/paged-result.model';

@Injectable({ providedIn: 'root' })
export class ContactService {
  private readonly http = inject(HttpClient);

  /** Same-origin in every environment: ng serve proxies /api to the backend, nginx does in production. */
  private readonly baseUrl = '/api/contacts';

  getContacts(query: ContactListQuery): Observable<PagedResult<Contact>> {
    let params = new HttpParams()
      .set('page', query.page)
      .set('pageSize', query.pageSize);

    for (const field of ['firstName', 'lastName', 'address', 'phoneNumber'] as const) {
      const term = query[field]?.trim();

      if (term) {
        params = params.set(field, term);
      }
    }

    return this.http.get<PagedResult<Contact>>(this.baseUrl, { params });
  }

  getContact(id: string): Observable<Contact> {
    return this.http.get<Contact>(`${this.baseUrl}/${id}`);
  }

  createContact(request: ContactRequest): Observable<Contact> {
    return this.http.post<Contact>(this.baseUrl, request);
  }

  updateContact(id: string, request: ContactRequest): Observable<Contact> {
    return this.http.put<Contact>(`${this.baseUrl}/${id}`, request);
  }

  deleteContact(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}
