import { Component, DestroyRef, OnInit, inject } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { NonNullableFormBuilder, ReactiveFormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
import { EMPTY, Subject, catchError, debounceTime, startWith, switchMap, tap } from 'rxjs';
import { Contact } from '../../../core/models/contact.model';
import { ContactService } from '../../../core/services/contact.service';

@Component({
  selector: 'app-contact-list',
  imports: [ReactiveFormsModule, RouterLink, NgbPaginationModule],
  templateUrl: './contact-list.component.html',
})
export class ContactListComponent implements OnInit {
  private readonly contactService = inject(ContactService);
  private readonly formBuilder = inject(NonNullableFormBuilder);
  private readonly router = inject(Router);
  private readonly destroyRef = inject(DestroyRef);

  private readonly reload$ = new Subject<void>();

  readonly pageSize = 10;
  page = 1;
  totalCount = 0;
  contacts: Contact[] = [];
  loading = true;

  readonly searchForm = this.formBuilder.group({
    firstName: '',
    lastName: '',
    address: '',
    phoneNumber: '',
  });

  ngOnInit(): void {
    this.searchForm.valueChanges
      .pipe(debounceTime(300), takeUntilDestroyed(this.destroyRef))
      .subscribe(() => {
        this.page = 1;
        this.reload$.next();
      });

    this.reload$
      .pipe(
        startWith(undefined),
        tap(() => (this.loading = true)),
        switchMap(() =>
          this.contactService
            .getContacts({ ...this.searchForm.getRawValue(), page: this.page, pageSize: this.pageSize })
            .pipe(
              catchError(() => {
                this.loading = false;
                return EMPTY;
              }),
            ),
        ),
        takeUntilDestroyed(this.destroyRef),
      )
      .subscribe((result) => {
        this.contacts = result.items;
        this.totalCount = result.totalCount;
        this.page = result.page;
        this.loading = false;
      });
  }

  onPageChange(page: number): void {
    this.page = page;
    this.reload$.next();
  }

  openContact(contact: Contact): void {
    this.router.navigate(['/contacts', contact.id]);
  }
}
