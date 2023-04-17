import React, { Component } from 'react';
import {
  Collapse,
  DropdownItem, DropdownMenu, DropdownToggle,
  Navbar,
  NavbarBrand,
  NavbarToggler,
  NavItem,
  NavLink,
  UncontrolledDropdown
} from 'reactstrap';
import { Link } from 'react-router-dom';
import './NavMenu.css';

export class NavMenu extends Component {
  static displayName = NavMenu.name;

  constructor (props) {
    super(props);

    this.toggleNavbar = this.toggleNavbar.bind(this);
    this.state = {
      collapsed: true
    };
  }

  toggleNavbar () {
    this.setState({
      collapsed: !this.state.collapsed
    });
  }

  render() {
    return (
      <header>
        <Navbar className="navbar-expand-sm navbar-toggleable-sm ng-white border-bottom box-shadow mb-3" container light>
          <NavbarBrand tag={Link} to="/">
            <img id="site-image" src="cropped-Linear-Black-2.png"/>
          </NavbarBrand>
          <NavbarToggler onClick={this.toggleNavbar} className="mr-2" />
          <Collapse className="d-sm-inline-flex flex-sm-row-reverse" isOpen={!this.state.collapsed} navbar>
            <ul className="navbar-nav flex-grow">
              <NavItem>
                <NavLink tag={Link} className="text-dark" to="/">Coins</NavLink>
              </NavItem>
              <UncontrolledDropdown nav inNavbar>
                <DropdownToggle nav caret className="text-dark">
                  Rankings
                </DropdownToggle>
                <DropdownMenu right>
                  <DropdownItem>
                    <NavLink tag={Link} className="text-dark" to="/members-ranking">Members</NavLink>
                  </DropdownItem>
                  <DropdownItem>
                    <NavLink tag={Link} className="text-dark" to="/bases-ranking">Bases</NavLink>
                  </DropdownItem>
                </DropdownMenu>
              </UncontrolledDropdown>
              <UncontrolledDropdown nav inNavbar>
                <DropdownToggle nav caret className="text-dark">
                  Users
                </DropdownToggle>
                <DropdownMenu right>
                  <DropdownItem>
                    <NavLink tag={Link} className="text-dark" to="/new-user">New</NavLink>
                  </DropdownItem>
                  <DropdownItem>
                    <NavLink tag={Link} className="text-dark" to="/users">List</NavLink>
                  </DropdownItem>
                </DropdownMenu>
              </UncontrolledDropdown>              
            </ul>
          </Collapse>
        </Navbar>
      </header>
    );
  }
}
