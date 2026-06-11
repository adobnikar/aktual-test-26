import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit, inject } from '@angular/core';
import { NonNullableFormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { ContactRequest } from '../../../core/models/contact.model';
import { ContactService } from '../../../core/services/contact.service';

const PHONE_NUMBER_PATTERN = /^\+?[0-9 ()\-./]*[0-9][0-9 ()\-./]*$/;

const FIELD_LABELS: Record<string, string> = {
  firstName: 'First name',
  lastName: 'Last name',
  address: 'Address',
  phoneNumber: 'Phone number',
};

@Component({
  selector: 'app-contact-form',
  imports: [ReactiveFormsModule, RouterLink],
  templateUrl: './contact-form.component.html',
})
export class ContactFormComponent implements OnInit {
  private readonly contactService = inject(ContactService);
  private readonly formBuilder = inject(NonNullableFormBuilder);
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);

  contactId: string | null = null;
  saving = false;

  readonly form = this.formBuilder.group({
    firstName: ['', [Validators.required, Validators.maxLength(100)]],
    lastName: ['', [Validators.required, Validators.maxLength(100)]],
    address: ['', [Validators.required, Validators.maxLength(250)]],
    phoneNumber: ['', [Validators.required, Validators.maxLength(30), Validators.pattern(PHONE_NUMBER_PATTERN)]],
  });

  get isEditMode(): boolean {
    return this.contactId !== null;
  }

  ngOnInit(): void {
    this.contactId = this.route.snapshot.paramMap.get('id');

    if (this.contactId) {
      this.contactService.getContact(this.contactId).subscribe({
        next: (contact) => this.form.patchValue(contact),
        error: () => this.router.navigate(['/contacts']),
      });
    }
  }

  submit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const value = this.form.getRawValue();
    const request: ContactRequest = {
      firstName: value.firstName.trim(),
      lastName: value.lastName.trim(),
      address: value.address.trim(),
      phoneNumber: value.phoneNumber.trim(),
    };

    this.saving = true;

    const save$ = this.contactId
      ? this.contactService.updateContact(this.contactId, request)
      : this.contactService.createContact(request);

    save$.subscribe({
      next: (contact) => this.router.navigate(['/contacts', contact.id]),
      error: (error: HttpErrorResponse) => {
        this.saving = false;
        this.applyServerErrors(error);
      },
    });
  }

  isInvalid(field: string): boolean {
    const control = this.form.get(field);

    return !!control && control.touched && control.invalid;
  }

  errorMessage(field: string): string | null {
    const control = this.form.get(field);

    if (!control?.errors) {
      return null;
    }

    const label = FIELD_LABELS[field];

    if (control.errors['required']) {
      return `${label} is required.`;
    }

    if (control.errors['maxlength']) {
      return `${label} may be at most ${control.errors['maxlength'].requiredLength} characters long.`;
    }

    if (control.errors['pattern']) {
      return `${label} must be a valid phone number.`;
    }

    if (control.errors['duplicate']) {
      return 'A contact with this phone number already exists.';
    }

    if (control.errors['server']) {
      return control.errors['server'];
    }

    return `${label} is invalid.`;
  }

  /** Maps backend validation problems (400) and phone-number conflicts (409) onto form controls. */
  private applyServerErrors(error: HttpErrorResponse): void {
    if (error.status === 409) {
      const control = this.form.controls.phoneNumber;
      control.setErrors({ ...control.errors, duplicate: true });
      control.markAsTouched();
      return;
    }

    if (error.status === 400 && error.error?.errors) {
      for (const [field, messages] of Object.entries<string[]>(error.error.errors)) {
        const control = this.form.get(field);

        if (control) {
          control.setErrors({ ...control.errors, server: messages[0] });
          control.markAsTouched();
        }
      }
    }
  }
}
