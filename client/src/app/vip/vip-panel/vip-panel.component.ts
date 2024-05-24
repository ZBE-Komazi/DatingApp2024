import { Component, OnInit } from '@angular/core';
import { Member } from 'src/app/_models/member';
import { Pagination } from 'src/app/_models/pagination';
import { MembersService } from 'src/app/_services/members.service';

@Component({
  selector: 'app-vip-panel',
  templateUrl: './vip-panel.component.html',
  styleUrls: ['./vip-panel.component.css']
})
export class VipPanelComponent implements OnInit {
  members: Member[] | undefined;
  predicate = 'visited';
  pageNumber = 1;
  pageSize = 5;
  pagination: Pagination | undefined;

  constructor(private memberService: MembersService) { }

  ngOnInit(): void {
    this.loadVisits();
  }

  loadVisits() {
    this.memberService.getVisits(this.predicate, this.pageNumber, this.pageSize).subscribe({
      next: response => {
        this.members = response.result;
        this.pagination = response.pagination;
      }
    })
  }

  pageChanged(event: any) {
    if (this.pageNumber !== event.page) {
      this.pageNumber = event.page;
      this.loadVisits();
    }
  }
}
