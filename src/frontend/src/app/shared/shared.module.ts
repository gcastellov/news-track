import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { TranslateModule } from '@ngx-translate/core';
import { PaginatorComponent } from './paginator/paginator.component';
import { LoaderComponent } from './loader/loader.component';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    TranslateModule
  ],
  declarations: [PaginatorComponent, LoaderComponent],
  exports: [PaginatorComponent, LoaderComponent]
})
export class SharedModule { }
