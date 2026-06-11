/** Server-side search and pagination parameters for the contact list. */
export interface ContactListQuery {
  firstName?: string;
  lastName?: string;
  address?: string;
  phoneNumber?: string;
  page: number;
  pageSize: number;
}
