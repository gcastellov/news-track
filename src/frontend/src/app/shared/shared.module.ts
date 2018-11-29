import { NgModule } from '@angular/core';
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
    TranslateModule
  ],
  declarations: [PaginatorComponent, LoaderComponent, FailureComponent],
  exports: [PaginatorComponent, LoaderComponent, FailureComponent]
})
export class SharedModule { }
