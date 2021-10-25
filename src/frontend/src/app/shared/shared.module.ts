import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { TranslateModule } from '@ngx-translate/core';
import { PaginatorComponent } from './paginator/paginator.component';
import { LoaderComponent } from './loader/loader.component';
import { FailureComponent } from './failure/failure.component';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    TranslateModule.forChild({})
  ],
  declarations: [PaginatorComponent, LoaderComponent, FailureComponent],
  exports: [PaginatorComponent, LoaderComponent, FailureComponent, TranslateModule],
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class SharedModule { }
