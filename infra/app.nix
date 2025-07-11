{ config, pkgs, ... }:
{
  config = {
    environment.systemPackages = with pkgs; [
      git
      vim
      # https://taskfile.dev/
      go-task

    ];

    services.postgresql = {
      enable = true;
      ensureDatabases = [ "stikl" ];
      ensureUsers = [
        {
          name = "stikl-pod";
          ensureClauses = {
            login = true;
          };
        }
        {
          name = "cwb";
          ensureClauses = {
            login = true;
            superuser = true;
          };
        }
      ];
      enableTCPIP = true;
      authentication = pkgs.lib.mkOverride 10 ''
        #type database DBuser origin-address auth-method
        local all       all     trust
        # ipv4
        host  all      all     127.0.0.1/32   trust
        host  stikl    all     samenet        trust
      '';
    };

    # https://bkiran.com/blog/deploying-containers-nixos
    virtualisation = {
      podman = {
        enable = true;
      };

      oci-containers = {
        containers = {
          stikl-web = {
            login = {
              registry = "https://ghcr.io";
              username = "C0DK";
              passwordFile = "/etc/stikl/registry-password.txt";
            };
            image = "ghcr.io/c0dk/stikl:main";
            environment = {
              DEV_MODE = "false";
              ASPNETCORE_FORWARDEDHEADERS_ENABLED = "true";
            };
            environmentFiles = [
              ../app/.env
            ];
            ports = [ "8080:8080" ];
            pull = "always";

          };
        };
      };

    };
  };
}
