import { Routes } from '@angular/router';

export const routes: Routes = [
  { path: '', redirectTo: 'contacts', pathMatch: 'full' },
  {
    path: 'contacts',
    loadComponent: () =>
      import('./features/contacts/contact-list/contact-list.component').then((m) => m.ContactListComponent),
  },
  {
    path: 'contacts/new',
    loadComponent: () =>
      import('./features/contacts/contact-form/contact-form.component').then((m) => m.ContactFormComponent),
  },
  {
    path: 'contacts/:id',
    loadComponent: () =>
      import('./features/contacts/contact-detail/contact-detail.component').then((m) => m.ContactDetailComponent),
  },
  {
    path: 'contacts/:id/edit',
    loadComponent: () =>
      import('./features/contacts/contact-form/contact-form.component').then((m) => m.ContactFormComponent),
  },
  { path: '**', redirectTo: 'contacts' },
];
